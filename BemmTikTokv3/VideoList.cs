using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BemmTikTokv3
{
    public partial class VideoList
    {
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }

        [JsonProperty("extra")]
        public Extra Extra { get; set; }

        [JsonProperty("status_code")]
        public long StatusCode { get; set; }

        [JsonProperty("aweme_list")]
        public AwemeList[] AwemeList { get; set; }

        [JsonProperty("max_cursor")]
        public long MaxCursor { get; set; }

        [JsonProperty("min_cursor")]
        public long MinCursor { get; set; }
    }

    public partial class AwemeList
    {
        [JsonProperty("video_labels")]
        public object VideoLabels { get; set; }

        [JsonProperty("geofencing")]
        public object Geofencing { get; set; }

        [JsonProperty("label_top_text")]
        public object LabelTopText { get; set; }

        [JsonProperty("promotions")]
        public object Promotions { get; set; }

        [JsonProperty("video_text")]
        public object VideoText { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("cha_list")]
        public object ChaList { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("text_extra")]
        public TextExtra[] TextExtra { get; set; }

        [JsonProperty("image_infos")]
        public object ImageInfos { get; set; }

        [JsonProperty("comment_list")]
        public object CommentList { get; set; }

        [JsonProperty("aweme_id")]
        public string AwemeId { get; set; }

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }

        [JsonProperty("aweme_type")]
        public long AwemeType { get; set; }

        [JsonProperty("images")]
        public object Images { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("long_video")]
        public object LongVideo { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("avatar_thumb")]
        public AvatarLarger AvatarThumb { get; set; }

        [JsonProperty("follower_count")]
        public long FollowerCount { get; set; }

        [JsonProperty("custom_verify")]
        public string CustomVerify { get; set; }

        [JsonProperty("enterprise_verify_reason")]
        public string EnterpriseVerifyReason { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("rate")]
        public long Rate { get; set; }

        [JsonProperty("avatar_larger")]
        public AvatarLarger AvatarLarger { get; set; }

        [JsonProperty("followers_detail")]
        public object FollowersDetail { get; set; }

        [JsonProperty("type_label")]
        public object[] TypeLabel { get; set; }

        [JsonProperty("short_id")]
        public long ShortId { get; set; }

        [JsonProperty("aweme_count")]
        public long AwemeCount { get; set; }

        [JsonProperty("platform_sync_info")]
        public object PlatformSyncInfo { get; set; }

        [JsonProperty("secret")]
        public long Secret { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("follow_status")]
        public long FollowStatus { get; set; }

        [JsonProperty("with_shop_entry")]
        public bool WithShopEntry { get; set; }

        [JsonProperty("user_canceled")]
        public bool UserCanceled { get; set; }

        [JsonProperty("policy_version")]
        public object PolicyVersion { get; set; }

        [JsonProperty("sec_uid")]
        public string SecUid { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("is_ad_fake")]
        public bool IsAdFake { get; set; }

        [JsonProperty("is_gov_media_vip")]
        public bool IsGovMediaVip { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("favoriting_count")]
        public long FavoritingCount { get; set; }

        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        [JsonProperty("story_open")]
        public bool StoryOpen { get; set; }

        [JsonProperty("with_commerce_entry")]
        public bool WithCommerceEntry { get; set; }

        [JsonProperty("video_icon")]
        public AvatarLarger VideoIcon { get; set; }

        [JsonProperty("following_count")]
        public long FollowingCount { get; set; }

        [JsonProperty("total_favorited")]
        public long TotalFavorited { get; set; }

        [JsonProperty("geofencing")]
        public object Geofencing { get; set; }

        [JsonProperty("with_fusion_shop_entry")]
        public bool WithFusionShopEntry { get; set; }

        [JsonProperty("avatar_medium")]
        public AvatarLarger AvatarMedium { get; set; }

        [JsonProperty("has_orders")]
        public bool HasOrders { get; set; }

        [JsonProperty("verification_type")]
        public long VerificationType { get; set; }
    }

    public partial class AvatarLarger
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("url_list")]
        public Uri[] UrlList { get; set; }
    }

    public partial class Statistics
    {
        [JsonProperty("comment_count")]
        public long CommentCount { get; set; }

        [JsonProperty("digg_count")]
        public long DiggCount { get; set; }

        [JsonProperty("play_count")]
        public long PlayCount { get; set; }

        [JsonProperty("share_count")]
        public long ShareCount { get; set; }

        [JsonProperty("forward_count")]
        public long ForwardCount { get; set; }

        [JsonProperty("aweme_id")]
        public string AwemeId { get; set; }
    }

    public partial class TextExtra
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("hashtag_name")]
        public string HashtagName { get; set; }

        [JsonProperty("hashtag_id")]
        public long HashtagId { get; set; }
    }

    public partial class Video
    {
        [JsonProperty("origin_cover")]
        public AvatarLarger OriginCover { get; set; }

        [JsonProperty("ratio")]
        public string Ratio { get; set; }

        [JsonProperty("download_addr")]
        public AvatarLarger DownloadAddr { get; set; }

        [JsonProperty("has_watermark")]
        public bool HasWatermark { get; set; }

        [JsonProperty("play_addr_lowbr")]
        public AvatarLarger PlayAddrLowbr { get; set; }

        [JsonProperty("play_addr")]
        public AvatarLarger PlayAddr { get; set; }

        [JsonProperty("cover")]
        public AvatarLarger Cover { get; set; }



        [JsonProperty("vid")]
        public string Vid { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("dynamic_cover")]
        public AvatarLarger DynamicCover { get; set; }

        [JsonProperty("bit_rate")]
        public object BitRate { get; set; }
    }

    public partial class Extra
    {
        [JsonProperty("now")]
        public long Now { get; set; }

        [JsonProperty("logid")]
        public string Logid { get; set; }
    }
}
