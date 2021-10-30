using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace hockey_rest.Util
{
    public class HockeyUtil
    {
        #region querys

        /// <summary>
        /// recupera todas las tarjetas de creditos
        /// </summary>
        private const string QRY_CAMPEONATO_ACTIVO = "SELECT id_campeonato FROM campeonato WHERE activo = 'S'";

        #endregion

        /// <summary>
        /// Obtiene el id del campeonato activo
        /// </summary>
        /// <returns></returns>
        public static string ObtenerCampeonatoActivo()
        {
            try
            {
                string idCampeonato = string.Empty;

                var result = SqlServerUtil.ExecuteQueryDataSet(QRY_CAMPEONATO_ACTIVO);

                if (result != null)
                {
                    foreach (DataRow item in result)
                    {
                        idCampeonato = item[0].ToString();
                    }
                }

                return idCampeonato;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
