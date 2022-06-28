#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.ReworkTablet.Info
{
    public class TabletPlayerInfo : MonoBehaviour
    {
        [SerializeField] private RawImage avatarPreview;
        [SerializeField] private TextMeshProUGUI playerNameTMP;
        private const string DEFAULT_NAME = "NonSteam";

        private void Awake()
        {
            playerNameTMP.text = DEFAULT_NAME;
#if !DISABLESTEAMWORKS
            if (SteamManager.Initialized)
            {
                playerNameTMP.text = SteamFriends.GetPersonaName();
                avatarPreview.texture = GetSteamImageAsTexture2D(SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID()));
            }
#endif
        }
        
#if !DISABLESTEAMWORKS    
        private Texture2D GetSteamImageAsTexture2D(int iImage) {
            Texture2D ret = null;
            bool bIsValid = SteamUtils.GetImageSize(iImage, out var ImageWidth, out var ImageHeight);

            if (bIsValid) {
                byte[] img = new byte[ImageWidth * ImageHeight * 4];

                bIsValid = SteamUtils.GetImageRGBA(iImage, img, (int)(ImageWidth * ImageHeight * 4));
                if (bIsValid) {
                    ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
                    ret.LoadRawTextureData(img);
                    ret.Apply();
                }
            }

            return ret;
        }
#endif
    }
}