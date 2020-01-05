using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCF_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        
        public int Connect(string name)
        {
            ServerUser user = new ServerUser() {
                Id = nextId++,
                Name = name,
                operationContext = OperationContext.Current


            };
            SendMsg(user.Name + " : Connect in Chat",0);
            users.Add(user);
            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if(user != null)
            {
                users.Remove(user);
                SendMsg(user.Name + ":  Desconnect in Chat",0);
            }

        }

        public List<ServerUser> GetUsersConected()
        {
            return users;
        }

        public void SendMsg(string msg, int id)
        {
            foreach(var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }
        }
    }
}
