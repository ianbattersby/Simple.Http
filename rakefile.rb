include FileTest

TASKS = Rake.application.top_level_tasks

# Build information
SOLUTION_NAME = "Simple.Http"
SOLUTION_DESC = "A REST-focused, object-oriented Http Framework for .NET 4."
SOLUTION_LICENSE = "http://www.opensource.org/licenses/mit-license.php"
SOLUTION_URL = "http://github.com/markrendle/Simple.Http"
SOLUTION_COMPANY = "Mark Rendle, Ian Battersby, and contributors"
SOLUTION_COPYRIGHT = "Copyright (C) Mark Rendle 2012"

# Build configuration
load "VERSION.txt"

CONFIG = ENV["config"] || "Release"
PLATFORM = ENV["platform"] || "x86"
BUILD_NUMBER = "#{BUILD_VERSION}.#{(ENV["BUILD_NUMBER"] || Time.new.strftime('5%H%M'))}"
MONO = !RUBY_PLATFORM.match("linux|darwin").nil?
TEAMCITY = (!ENV["BUILD_NUMBER"].nil? or !ENV["TEAMCITY_BUILD_PROPERTIES_FILE"].nil?)
TEAMCITY_BRANCH = !TEAMCITY ? nil : ENV["BRANCH"]

# NuGet configuration
NUGET_APIKEY_LOCAL = ENV["apikey_local"]
NUGET_APIURL_LOCAL = ENV["apiurl_local"]
NUGET_APIKEY_REMOTE = ENV["apikey_remote"]
NUGET_APIURL_REMOTE = ENV["apiurl_remote"]
ENV["EnableNuGetPackageRestore"] = "true"

# Symbol server configuration
SYMBOL_APIURL_LOCAL = ENV["symbol_local"]
SYMBOL_APIURL_REMOTE = ENV["symbol_remote"] # For nuget.org make this the same as NUGET_APIURI_REMOTE

# Paths
BASE_PATH = File.expand_path(File.dirname(__FILE__))
SOURCE_PATH = "#{BASE_PATH}/src"
TESTS_PATH = "#{BASE_PATH}/src"
SPECS_PATH = "#{BASE_PATH}/specs"
BUILD_PATH = "#{BASE_PATH}/build"
RESULTS_PATH = "#{BASE_PATH}/results"
ARTIFACTS_PATH = "#{BASE_PATH}/artifacts"
NUSPEC_PATH = "#{BASE_PATH}/packaging/nuspec"
NUGET_PATH = "#{ARTIFACTS_PATH}/nuget"
TOOLS_PATH = "#{BASE_PATH}/tools"

# Files
ASSEMBLY_INFO = "#{SOURCE_PATH}/CommonAssemblyInfo.cs"
SOLUTION_FILE = "#{SOURCE_PATH}/Simple.Http.sln"
VERSION_INFO = "#{BASE_PATH}/VERSION.txt"

# Matching
TEST_ASSEMBLY_PATTERN_PREFIX = "Tests"
TEST_ASSEMBLY_PATTERN_UNIT = "#{TEST_ASSEMBLY_PATTERN_PREFIX}"
TEST_ASSEMBLY_PATTERN_INTEGRATION = "#{TEST_ASSEMBLY_PATTERN_PREFIX}.Integration"
SPEC_ASSEMBLY_PATTERN = ".Specs"
ROOT_NAMESPACE = ""

# Commands
# XUNIT_COMMAND = "#{TOOLS_PATH}/xunit/xunit.console.clr4.#{(PLATFORM.empty? or PLATFORM.eql?('x86') ? 'x86' : '')}.exe"
XUNIT_COMMAND = "#{TOOLS_PATH}/xUnit/xunit.console.clr4.exe"
MSPEC_COMMAND = "#{TOOLS_PATH}/mspec/mspec.exe"
NUGET_COMMAND = "#{SOURCE_PATH}/.nuget/NuGet.exe"

# Global vars
error_count = 0

# Set up our build system
require 'albacore'
require 'pathname'
require 'rake/clean'
require 'rexml/document'

# Check dependencies
raise "You do not have the required dependencies, run '.\\InstallGem.bat' or './installgem.sh'." \
    unless `bundle check`.include? "The Gemfile's dependencies are satisfied\n"

# Configure albacore
Albacore.configure do |config|
    config.log_level = (TEAMCITY ? :verbose : :normal)

    config.msbuild.solution = SOLUTION_FILE
    config.msbuild.properties = { :configuration => CONFIG }
    config.msbuild.use :net4
    config.msbuild.targets = [ :Clean, :Build ]
    config.msbuild.verbosity = "normal"

    config.xbuild.solution = SOLUTION_FILE
    config.xbuild.properties = { :configuration => CONFIG, :vstoolspath => (RUBY_PLATFORM.downcase.include?('darwin') ? '/Library/Frameworks/Mono.framework/Libraries' : '/usr/lib') + '/mono/xbuild/Microsoft/VisualStudio/v9.0' }
    config.xbuild.targets = [ :Build ] #:Clean upsets xbuild
    config.xbuild.verbosity = "normal"

    config.mspec.command = (MONO ? 'mono' : XUNIT_COMMAND)
    config.mspec.assemblies = FileList.new("#{SPECS_PATH}/**/*#{SPEC_ASSEMBLY_PATTERN}.dll").exclude(/obj\//).collect! { |element| ((MONO ? "#{MSPEC_COMMAND} " : '') + element) }

    CLEAN.include(FileList["#{SOURCE_PATH}/**/obj"])
    CLOBBER.include(NUGET_PATH)
	CLOBBER.include(FileList["#{SOURCE_PATH}/**/bin"])
    CLOBBER.include(ARTIFACTS_PATH)
	CLOBBER.include(BUILD_PATH)
	CLOBBER.include(RESULTS_PATH)
end

# Tasks
task :default => [:test]

desc "Build"
task :build => [:init, :assemblyinfo, :packagerestore] do
	if MONO
		Rake::Task[:xbuild].invoke
	else
		Rake::Task[:msbuild].invoke
	end
end

task :packagerestore do
    sh "#{(MONO ? 'mono ' : '')}#{NUGET_COMMAND} restore #{SOLUTION_FILE}"
end

desc "Build + Tests (default)"
task :test => [:build] do
    error_count = (error_count || 0) + RunTests("#{TEST_ASSEMBLY_PATTERN_PREFIX}*")
    raise "\nTest errors: #{error_count}\n" unless error_count == 0 || !TASKS.include?('test')
end

desc "Build + Unit tests"
task :quick => [:build] do
	error_count = (error_count || 0) + RunTests(TEST_ASSEMBLY_PATTERN_UNIT)
	raise "\nTest errors: #{error_count}\n" unless error_count == 0 || !TASKS.include?('quick')
end

desc "Build + Tests + Specs"
task :full => [:test] do #[:test, :mspec]
	raise "\nTest errors: #{error_count}\n" unless error_count == 0 || !TASKS.include?('full')
end

desc "Build + Tests + Specs + Publish (local)"
task :publocal => [:full] do
	raise "Environment variable \"APIURL_LOCAL\" must be a valid nuget server url." unless !NUGET_APIURL_LOCAL.nil?
	raise "Environment variable \"APIKEY_LOCAL\" must be that of your nuget api key." unless !NUGET_APIKEY_LOCAL.nil?

	PublishNugets BUILD_NUMBER, NUGET_APIURL_LOCAL, NUGET_APIKEY_LOCAL, SYMBOL_APIURL_LOCAL
end

desc "Build + Tests + Specs + Package"
task :package => [:full, :packageonly] 

task :packageonly do
	PackageNugets BUILD_NUMBER
end

desc "Build + Tests + Specs + Publish (remote)"
task :publish => [:full] do
	raise "Environment variable \"APIURL_REMOTE\" must be a valid nuget server url." unless !NUGET_APIURL_REMOTE.nil?
	raise "Environment variable \"APIKEY_REMOTE\" must be that of your nuget api key." unless !NUGET_APIKEY_REMOTE.nil?

	if not TEAMCITY
		puts "\n\nThis will publish your local build to the remote nuget feed. Are you sure (y/n)?"
		response = $stdin.gets.chomp

		raise "Publish aborted." unless response.downcase.eql?("y")
	end

	PublishNugets BUILD_NUMBER, NUGET_APIURL_REMOTE, NUGET_APIKEY_REMOTE, SYMBOL_APIURL_REMOTE

    Rake::Task[:tag].invoke()
end

# Hidden tasks
task :init => [:clobber] do
	if not TEAMCITY
        indexupdate = `git update-index --assume-unchanged #{SOURCE_PATH}/CommonAssemblyInfo.cs`
        raise "Unable to perform git index operation, cannot continue (#{indexupdate})." unless indexupdate.empty?
	end

	Dir.mkdir BUILD_PATH unless File.exists?(BUILD_PATH)
	Dir.mkdir RESULTS_PATH unless File.exists?(RESULTS_PATH)
	Dir.mkdir ARTIFACTS_PATH unless File.exists?(ARTIFACTS_PATH)
	Dir.mkdir NUGET_PATH unless File.exists?(NUGET_PATH)
end

task :tag do
    tagupdate = `git tag -a \"v#{BUILD_VERSION}\" -m \"Published nugets version #{BUILD_VERSION}.\"`
    raise "Unable to perform git tag operation (#{BUILD_VERSION})." unless tagupdate.empty?

    tagpush = `git push --tags` unless !tagupdate.empty?
    raise "Unable to push git tag changes." unless tagpush.empty?
end

task :ci => [:full] do #=> [:package]
	raise "\nTest errors: #{error_count}\n" unless error_count == 0 || !TASKS.include?('ci')
	Rake::Task[:packageonly].invoke()
end

msbuild :msbuild

xbuild :xbuild

task :assemblyinfo do
    @asm = AssemblyInfoCustom.new
    SetupAssemblyInfo(@asm)
    @asm.execute
end

mspec :mspec
	
# Helper methods
def RunTests(boundary = "*", stop_on_fail = !TEAMCITY)
	runner = XUnitTestRunnerCustom.new(MONO ? 'mono' : XUNIT_COMMAND)
	runner.html_output = RESULTS_PATH
    runner.skip_test_fail = !stop_on_fail

	assemblies = Array.new

    boundary.split(/,/).each do |this_boundary|
		FileList.new("#{TESTS_PATH}/*#{this_boundary}")
				.collect! { |element| 
					FileList.new("#{element}/**/*#{this_boundary}.dll")
						.exclude(/obj\//)
						.each do |this_file|
							assemblies.push (MONO ? "#{XUNIT_COMMAND} " : '') + this_file
						end
				}
    end

    if assemblies.length > 0
		runner.assemblies = assemblies
		runner.execute
	end

	return xunit_runner.error_count + (nunit_runner.nil? ? 0 : nunit_runner.error_count)
end

def PublishNugets(version, apiurl, apikey, symbolurl)
	PackageNugets(version)

	nupkgs = FileList["#{NUGET_PATH}/*#{$version}.nupkg"]
    nupkgs.each do |nupkg| 
        puts "Pushing #{Pathname.new(nupkg).basename}"
        nuget_push = NuGetPush.new
        nuget_push.source = "\"" + ((not symbolurl.nil? and nupkg.include?(".symbols.")) ? symbolurl :  apiurl) + "\""
		nuget_push.apikey = apikey
        nuget_push.command = NUGET_COMMAND
        nuget_push.package = (MONO ? nupkg : nupkg.gsub('/','\\'))
        nuget_push.create_only = false
        nuget_push.execute
    end
end

def PackageNugets(nuspec_version)
	raise "Invalid nuspec version specified." unless !nuspec_version.nil?

	Dir.mkdir "#{ARTIFACTS_PATH}/nuspec" unless File.exists?("#{ARTIFACTS_PATH}/nuspec")

    FileUtils.cp_r FileList["#{NUSPEC_PATH}/**/*.nuspec"], "#{ARTIFACTS_PATH}/nuspec"

    nuspecs = FileList["#{ARTIFACTS_PATH}/nuspec/**/*.nuspec"]

	UpdateNuSpecVersions nuspecs, nuspec_version

    nuspecs.each do |nuspec|      
        nuget = NuGetPackCustom.new
        nuget.command = NUGET_COMMAND
        nuget.nuspec = nuspec
        nuget.output = NUGET_PATH
        nuget.base_folder = NUSPEC_PATH
        nuget.execute
    end
end

def UpdateNuSpecVersions(nuspecs, target_version)
	raise "No nuspecs to update." unless !nuspecs.nil?
	raise "Invalid nuspec version specified." unless !target_version.nil?

    suffix = ""
    suffix << "-#{TEAMCITY_BRANCH}" unless (TEAMCITY_BRANCH.nil? or TEAMCITY_BRANCH.eql? "master" or TEAMCITY_BRANCH.eql? "<default>")
    suffix << "-mono" unless !MONO

    nuspecs.each do |nuspec|
        puts "Updating #{Pathname.new(nuspec).basename}"
        update_xml nuspec do |xml|
            nuspec_id = xml.root.elements["metadata/id"].text
            nuspec_version = xml.root.elements["metadata/version"].text
            nuspec_mm_version = "[#{target_version.split(".").first(4).join(".")}]"
            target_mm_version = target_version.split(".")

            xml.root.elements["metadata/id"].text = nuspec_id + suffix 
            xml.root.elements["metadata/version"].text = !nuspec_version.include?("-") ? target_version : target_mm_version.first(3).join(".") + (nuspec_version.include?("-") ? "-#{nuspec_version.partition('-').last}" : "") + "-#{target_mm_version.last}"
            xml.root.elements["metadata/summary"].text = SOLUTION_DESC
            xml.root.elements["metadata/licenseUrl"].text = SOLUTION_LICENSE
            xml.root.elements["metadata/projectUrl"].text = SOLUTION_URL

            if xml.root.elements["metadata/authors"].include?('$authors$')
                xml.root.elements["metadata/authors"].text = SOLUTION_COMPANY
	        end

			xml.root.get_elements("//dependency").each { |e|
				if e.attributes["id"].downcase.include? SOLUTION_NAME.downcase
					e.attributes["id"] = (e.attributes["id"] + suffix)
					e.attributes["version"] = "#{nuspec_mm_version}"
				end
			}

			xml.root.get_elements("//file").each { |e| 
				e.attributes["src"] = e.attributes["src"].gsub((MONO ? '\\' : '/'), (!MONO ? '\\' : '/')) 
				e.attributes["target"] = e.attributes["target"].gsub((MONO ? '\\' : '/'), (!MONO ? '\\' : '/'))
			}
        end
    end
end

def SetupAssemblyInfo(asm)
	asm_version = BUILD_NUMBER

    if TEAMCITY
        puts "##teamcity[buildNumber '#{BUILD_NUMBER}']"
    end

	begin
		commit = `git log -1 --pretty=format:%H`
	rescue
		commit = "git unavailable"
	end

	testassemblies = FileList.new("#{TESTS_PATH}/*#{TEST_ASSEMBLY_PATTERN_PREFIX}.*/", "#{TESTS_PATH}/*#{TEST_ASSEMBLY_PATTERN_PREFIX}/")
		.pathmap("%f")
		.collect! { |assemblyname|
			"[assembly: InternalsVisibleTo(\"#{ROOT_NAMESPACE}#{assemblyname}\")]"
		}

	asm.language = "C#"
	asm.version = BUILD_NUMBER
	asm.trademark = commit
	asm.file_version = BUILD_NUMBER
	asm.company_name = SOLUTION_COMPANY
	asm.product_name = SOLUTION_NAME
	asm.copyright = SOLUTION_COPYRIGHT
	asm.namespaces 'System.Runtime.CompilerServices'
	asm.custom_attributes :AssemblyConfiguration => CONFIG, :AssemblyInformationalVersion => asm_version
	asm.custom_data testassemblies
	asm.output_file = ASSEMBLY_INFO
	asm.com_visible = false
end

def update_xml(xml_path)
    xml_file = File.new(xml_path)
    xml = REXML::Document.new xml_file
 
    yield xml
 
    xml_file.close
         
    xml_file = File.open(xml_path, "w")
    formatter = REXML::Formatters::Default.new(5)
    formatter.write(xml, xml_file)
    xml_file.close 
end

# Albacore needs some Mono help
class XUnitTestRunnerCustom
    TaskName = :xunit
    include Albacore::Task
    include Albacore::RunCommand

    attr_accessor :html_output, :skip_test_fail, :error_count
    attr_array :options,:assembly,:assemblies

    def initialize(command=nil)
        @options=[]
        super()
        update_attributes Albacore.configuration.xunit.to_hash
        @command = command unless command.nil?
        @error_count = 0
    end

    def get_command_line
    command_params = []
    command_params << @command
    command_params << get_command_parameters
    commandline = command_params.join(" ")
    @logger.debug "Build XUnit Test Runner Command Line: " + commandline
    commandline
    end

    def get_command_parameters
    command_params = [] 
    command_params << @options.join(" ") unless @options.nil?
    command_params << build_html_output unless @html_output.nil?
    command_params
    end

    def execute()         
        @assemblies = [] if @assemblies.nil?
        @assemblies << @assembly unless @assembly.nil?
        fail_with_message 'At least one assembly is required for assemblies attr' if @assemblies.length==0  
        failure_message = 'XUnit Failed. See Build Log For Detail'      

        @assemblies.each do |assm|
          command_params = get_command_parameters.collect{ |p| p % File.basename(assm) }
          command_params.insert(0,assm) 
          result = run_command "XUnit", command_params.join(" ")
          @error_count = (@error_count + $?.exitstatus) if !result && $?.exitstatus > 1
          fail_with_message failure_message if !result && !@skip_test_fail
        end       
    end

    def build_html_output
        fail_with_message 'Directory is required for html_output' if !File.directory?(File.expand_path(@html_output))
        "/nunit \"@#{File.join(File.expand_path(@html_output),"%s.html")}\""
    end
end

class NuGetPackCustom < NuGetPack
  def execute  
    fail_with_message 'nuspec must be specified.' if @nuspec.nil?
    
    params = []
    params << "pack"
    params << "-Symbols" if !TEAMCITY
    params << nuspec
    params << "-BasePath \"#{base_folder}\"" unless @base_folder.nil?
    params << "-OutputDirectory \"#{output}\"" unless @output.nil?
    params << "-NoDefaultExcludes" unless !MONO
    params << "-Verbosity detailed" unless !TEAMCITY
    params << build_properties unless @properties.nil? || @properties.empty?
    
    merged_params = params.join(' ')
    
    cmd = "#{MONO ? 'mono ' : ''}#{File.expand_path(@command)} #{merged_params}"
    result = false

    @logger.debug "Build NuGet pack Command Line: #{cmd}"

	Dir.chdir(@working_directory) do
      result = system(cmd)
    end

    failure_message = 'NuGet Failed. See Build Log For Detail'
    fail_with_message failure_message if !result
  end
end

class NuGetPublishCustom < NuGetPackCustom
  def execute
  
    fail_with_message 'id must be specified.' if @id.nil?
    fail_with_message 'version must be specified.' if @version.nil?
    # don't validate @apikey as required, coz it might have been set in the config file using 'SetApiKey'
    
    puts @create_only
    params = []
    params << "publish"
    params << "#{@id}"
    params << "#{@version}"
    params << "#{@apikey}" if @apikey
    params << "-Source #{source}" unless @source.nil?
    
    merged_params = params.join(' ')
    
    cmd = "#{MONO ? 'mono ' : ''}#{File.expand_path(@command)} #{merged_params}"
    result = false

    @logger.debug "Build NuGet publish Command Line: #{cmd}"

	Dir.chdir(@working_directory) do
      result = system(cmd)
    end
    
    failure_message = 'NuGet Publish Failed. See Build Log For Details'
    fail_with_message failure_message if !result
  end
end

class AssemblyInfoCustom < AssemblyInfo
  include Albacore::Task
  include Configuration::AssemblyInfo

  def build_using_statements(data)
    @namespaces = [] if @namespaces.nil?
    
    @namespaces << "System.Reflection"
    @namespaces << "System.Runtime.InteropServices"
    @namespaces.uniq!
    
    # This is the only change made to the default behaviour. It's so we can push out an auto-generated marker
    # at the top of the file, which gets StyleCop to ignore it.
    ns = ["// <auto-generated/>"]
    @namespaces.each do |n|
      ns << @lang_engine.build_using_statement(n) unless data.index { |l| l.match n }
    end
    
    ns
  end
end