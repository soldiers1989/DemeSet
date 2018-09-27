namespace ZF.Infrastructure.AlipayService.Model
{
    public class VideoModel
    {
        /// <summary>
        /// 请求ID。
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 视频播放凭证
        /// </summary>
        public string PlayAuth { get; set; }

        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// 视频封面
        /// </summary>
        public string CoverURL { get; set; }

        /// <summary>
        /// 视频时长
        /// </summary>
        public float? Duration { get; set; }

        /// <summary>
        /// 视频状态
        /// Uploading	上传中	视频的初始状态，表示正在上传。
        /// UploadFail	上传失败	由于是断点续传，无法确定上传是否失败，故暂不会出现此值。
        /// UploadSucc	上传完成	-
        /// Transcoding	转码中	-
        /// TranscodeFail	转码失败	转码失败，一般是原片有问题，可在事件通知的 转码完成消息 得到ErrorMessage失败信息，或提交工单联系我们。
        /// Checking	审核中	在 视频点播控制台 > 全局设置 > 审核设置 开启了 先审后发，转码成功后视频状态会变成审核中，此时视频只能在控制台播放。
        /// Blocked	屏蔽	在审核时屏蔽视频。
        /// Normal	正常	视频可正常播放。
        /// </summary>
        public string Status { get; set; }
    }
}