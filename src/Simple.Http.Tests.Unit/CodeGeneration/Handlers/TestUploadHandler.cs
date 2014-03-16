namespace Simple.Http.Tests.Unit.CodeGeneration.Handlers
{
    using System.Collections.Generic;

    using Simple.Http.Behaviors;

    class TestUploadHandler : IPost, IUploadFiles
    {
        public Status Post()
        {
            return 200;
        }

        public IEnumerable<IPostedFile> Files
        {
            set { }
        }
    }
}