namespace ZF.Infrastructure.AlipayService.Model
{
    public class UploadVideo
    {
        public string RequestId { get; set; }
        public string UploadAddress { get; set; }
        public string UploadAuth { get; set; }
        public string VideoId { get; set; }
    }
}