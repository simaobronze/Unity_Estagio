using System.Linq;
using System.Transactions;
using UnityEngine;

[System.Serializable]
public class CreateSessionRequest : JanusRequest
{
        public CreateSessionRequest()
        {
                janus = "create";
        }


}
