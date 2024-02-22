using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorePush.Google;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using static Nursery.Entities.Notification.GoogleNotification;

namespace Nursery.Entities.Notification
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
    }

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        public NotificationService(IOptions<FcmNotificationSetting> settings)
        {
            _fcmNotificationSetting = settings.Value;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (notificationModel.IsAndroiodDevice)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings()
                    {
                       // SenderId = "656285146411",
                        SenderId = "420676963556",
                        //ServerKey = "AAAAmM2knSs:APA91bEfJD3PrridayFM1aSSjQ6GdGf8yuEeoiWxuaG4jPo3Zp2bIx9tZ88TEdHVAKVbgOCnOCgU_qs2_rDnnyjnVPXc3DUQfUSEHnakOCJwRdZC_qFABUTlrUmPgKwJLRJIoaXrx1OD"
                        ServerKey = "AAAAYfJNDOQ:APA91bHtMhvASZx3m0hPxsjucBFHcJ_WGLs_-gncLl0cIImWTnPS6IZXURGnHs1a5Knn4hRcfgbq8KLjaKAj9qwCOwIPevPudur-cTq4oZOLBC24p525P10IckVKav9J3uHLpMRk3OXi"
                    };

                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = notificationModel.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;
                    dataPayload.EntityId = notificationModel.EntityId;
                    //dataPayload.EntityTypeId = notificationModel.EntityTypeId;
                    dataPayload.EntityTypeNotifyId = notificationModel.EntityTypeNotifyId;

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync   (deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }
    }
}
