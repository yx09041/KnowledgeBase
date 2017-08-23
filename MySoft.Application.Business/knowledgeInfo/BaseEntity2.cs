using System;
namespace MySoft.Application.Business
{
    [Serializable]
    public partial class BaseEntity2
    {
        public JobType JobType { get; set; }
        private string sourceName = string.Empty;

        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }

    }
}