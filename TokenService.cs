using Sabio.Data;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class TokenService : BaseService
    {
        //POST:
        public static Guid Post(string UserId)                                                              //Using a GUID to target the UserId from the DB.
        {
            Guid newToken = Guid.Empty;                                                                     //Guid must be set to empty. //outputEmail

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Token_Insert"                                  //Link to the correct store proc.
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)                        //Delegate takes 3 parameters with variables: @UserId, @TokenType
               {
                   paramCollection.AddWithValue("@UserId", UserId);                                         
                   paramCollection.AddWithValue("@TokenType", 1);

                   SqlParameter p = new SqlParameter("@Token", System.Data.SqlDbType.UniqueIdentifier);     //& @Token which is the GUID string.
                   p.Direction = System.Data.ParameterDirection.Output;
                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)                                 
               {
                   Guid.TryParse(param["@Token"].Value.ToString(), out newToken);
               }
               );          

            return newToken;
        }

        //GET BY TOKEN:
        public static Token GetByToken(Guid Token)                                      
        {
            Token p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Token_SelectByToken"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Token", Token);
               }, map: delegate (IDataReader reader, short set)
               {
                   int startingIndex = 0; //startingOrdinal

                   p = new Token();                
                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.DateAdded = reader.GetSafeDateTime(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.TokenString = reader.GetSafeGuid(startingIndex++);
                   p.TokenType = reader.GetSafeInt32(startingIndex++);
                   p.IsUsed = reader.GetSafeBool(startingIndex++);
                   p.DateUsed = reader.GetSafeDateTime(startingIndex++);
               }
               );

            return p;
        }


        //UPDATE TOKEN TO USED:
        public static void MarkedIsUsed(Token model)                                        //To expire the token, so it cannot be used again.
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Token_UpdateUsed"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Token", model.TokenString);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );
        }
    }
}