using SQLAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SQLAPI.Controllers
{
    [Authorize]
    public class CountryController : ApiController
    {
        private IEnumerable<Country> QueryCountries(string sql)
        {
            List<Country> listCountry = new List<Country>();

            string cs = ConfigurationManager.ConnectionStrings["DW"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sql;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listCountry.Add(new Country
                    {
                        id = Convert.ToInt32(rdr["ID"]),
                        code = rdr["Code"].ToString(),
                        name = rdr["Name"].ToString()
                    });
                }
            }

            return listCountry;
        }
        // GET: api/Country
        public IEnumerable<Country> Get()
        {
            string sqlText = "SELECT ID, Code, Name " +
                             "FROM dbo.DimCountry " +
                             "ORDER BY Code";

            return QueryCountries(sqlText).ToArray();
        }
    }
}
