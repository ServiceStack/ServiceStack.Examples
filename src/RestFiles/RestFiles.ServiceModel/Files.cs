using RestFiles.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;

namespace RestFiles.ServiceModel
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary> 
    [Api("GET the File or Directory info at {Path}\n"
        + "POST multipart/formdata to upload a new file to any {Path} in the /ReadWrite folder\n"
        + "PUT {TextContents} to replace the contents of a text file in the /ReadWrite folder\n")]
    [Route("/files")]
    [Route("/files/{Path*}")]	
    public class Files
    {		
        public string Path { get; set; }		
        public string TextContents { get; set; }		
        public bool ForDownload { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class FilesResponse : IHasResponseStatus
    {		
        public FolderResult Directory { get; set; }		
        public FileResult File { get; set; }

        /// <summary>
        /// Gets or sets the ResponseStatus. The built-in IoC used with ServiceStack autowires this property.
        /// </summary>		 
        public ResponseStatus ResponseStatus { get; set; }
    }
}