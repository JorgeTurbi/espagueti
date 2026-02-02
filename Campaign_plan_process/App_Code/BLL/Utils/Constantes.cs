using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign_plan_process
{
    public class Constantes
    {
        #region Enumeración para Status Campaña

        public enum status_campaign
        {
            Planificado = 1,
            Proceso = 2,
            Finalizado = 3,
            Reenvio_open = 4,
            Reenvio_no_opens = 5,
            Reenvio_clicks = 6
        }

        #endregion

        #region Enumeración para usuario especial mails

        public enum usuario_especial_mail
        {
            Aniacam = 19820,
            Sbs_Comunicacion = 19819,
            Munuslingua = 47066
        }

        #endregion

        #region Enumeración para Status Mail

        public enum status_mail
        {
            Not_Send = 0,
            Send = 1,
            Open = 2,
            Bounced = 3,
            Clic = 4,
            Reenviado = 5
        }

        #endregion

        #region Enumeración para Status Mail Type

        public enum type_status_mail
        {
            Open = 1,
            No_Open = 2,
            Clic = 3
        }

        #endregion
    }
}
