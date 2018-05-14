using Carrotcord_API.Carrotcord.Stuff;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class JSONDeserializeAndHandleErrors
    {

        public static dynamic DeserializeJSON(IRestResponse response)
        {
            dynamic data = JsonConvert.DeserializeObject(response.Content);

            try {
                if (data.code != null) { throw new JSONError((ErrorCode)Convert.ToInt32(data.code), Convert.ToString(data.message)); }
                else return data;
            } catch(RuntimeBinderException)
            {
                return data;
            }

            /**DoubleVar<dynamic, ErrorCode> converted = checkForError(response.Content);
            CarrotcordLogger.log(CarrotcordLogger.LogSource.ERRORHANDLER, "" + converted.obj1);*/
            /**Exception exception = getExceptionFromErrorCode(converted.obj1);
            if (exception != null) throw exception;*/
            //return converted.obj;
        }

        public static DoubleVar<dynamic, ErrorCode> checkForError(string rawData)
        {
            dynamic data = JsonConvert.DeserializeObject(rawData);
            if (data.code != null)
            {
                return new DoubleVar<dynamic, ErrorCode>(null, (ErrorCode)Convert.ToInt32(data.code));
            }
            else return new DoubleVar<dynamic, ErrorCode>(data, ErrorCode.OK);
        }

        public static Exception getExceptionFromErrorCode(ErrorCode code)
        {
            //return new JSONError(code, "dd");
            switch (code)
            {
                case ErrorCode.OK:
                    return null;
                case ErrorCode.MISSING_ACCESS:
                    return new MissingAccessException50001();
                case ErrorCode.UNKNOWN_MEMBER:
                    return new UnknownMemberException10007();
            }
            return null;
        }

    }

    public class DoubleVar<A, B>
    {
        public A obj;
        public B obj1;
        public DoubleVar(A obj, B obj1)
        {
            this.obj = obj;
            this.obj1 = obj1;
        }
    }

}
