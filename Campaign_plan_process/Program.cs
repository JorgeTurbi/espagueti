using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campaign_plan_process
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccess da = new DataAccess();

            try
            {
                Console.WriteLine("Inicio del proceso");
                LogUtils.InsertarLog("Inicio del proceso", true);

                /// 1.- Comprobar si hay alguna campaña planificada
                List<EMAIL_CAMPAIGNS> lst_campaigns = da.getCampaignsByStatus((int)Constantes.status_campaign.Planificado);
                if (lst_campaigns.Count > 0)
                {
                    /// 1.1.- Coger una campaña y cambiar el estado a en proceso
                    lst_campaigns = lst_campaigns.Take(1).ToList();
                    if (lst_campaigns.Count == 1)
                    {
                        EMAIL_CAMPAIGNS campaign = lst_campaigns[0];
                        bool update_status = da.updateStatusPlanEmailCampaigns(campaign, (int)Constantes.status_campaign.Proceso);
                        if (update_status)
                        {
                            /// 1.2.- Planificar el envío de la campaña
                            Planificar_newsletter plan = new Planificar_newsletter();
                            bool process = plan.process(campaign);
                            if (process)
                            {
                                /// 1.3.- Cambiar el status a finalizado
                                List<EMAIL_CAMPAIGNS> list_campaigns = da.getCampaignsById(campaign.id_camp);
                                if (list_campaigns.Count == 1)
                                {
                                    EMAIL_CAMPAIGNS _campaign = list_campaigns[0];
                                    bool update_status_campaign = da.updateStatusPlanEmailCampaigns(_campaign, (int)Constantes.status_campaign.Finalizado);
                                    if (!update_status_campaign)
                                        LogUtils.InsertarLog(" ERROR - Al actualizar el status finalizado en EMAIL_CAMPAIGNS", true);
                                }
                            }
                        }
                        else
                            LogUtils.InsertarLog(" ERROR - Al actualizar el status en EMAIL_CAMPAIGNS", true);
                    }
                }

                /// 2.- Comprobar si hay alguna campaña de reenvío no opens
                List<EMAIL_CAMPAIGNS> lst_campaigns_no_opens = da.getCampaignsByStatus((int)Constantes.status_campaign.Reenvio_no_opens);
                if (lst_campaigns_no_opens.Count > 0)
                {
                    /// 2.1.- Coger una campaña y cambiar el estado a en proceso
                    lst_campaigns_no_opens = lst_campaigns_no_opens.Take(1).ToList();
                    if (lst_campaigns_no_opens.Count == 1)
                    {
                        EMAIL_CAMPAIGNS campaign = lst_campaigns_no_opens[0];
                        bool update_status = da.updateStatusPlanEmailCampaigns(campaign, (int)Constantes.status_campaign.Proceso);
                        if (update_status)
                        {
                            /// 2.2.- Planificar el envío de la campaña
                            Planificar_newsletter plan = new Planificar_newsletter();
                            bool process = plan.process_no_opens(campaign);
                            if (process)
                            {
                                /// 2.3.- Cambiar el status a finalizado
                                List<EMAIL_CAMPAIGNS> list_campaigns = da.getCampaignsById(campaign.id_camp);
                                if (list_campaigns.Count == 1)
                                {
                                    EMAIL_CAMPAIGNS _campaign = list_campaigns[0];
                                    bool update_status_campaign = da.updateStatusPlanEmailCampaigns(_campaign, (int)Constantes.status_campaign.Finalizado);
                                    if (!update_status_campaign)
                                        LogUtils.InsertarLog(" ERROR - Al actualizar el status finalizado en EMAIL_CAMPAIGNS", true);
                                }
                            }
                        }
                        else
                            LogUtils.InsertarLog(" ERROR - Al actualizar el status en EMAIL_CAMPAIGNS", true);
                    }
                }

                /// 3.- Comprobar si hay alguna campaña planificada por activadores
                List<EMAIL_ACTIVADORES> list_activators = da.getActivatorsByProcess(false, DateTime.Now);
                if (list_activators.Count > 0)
                {
                    /// 3.0.- Filtrar los activadores
                    List<EMAIL_ACTIVADORES> lst_activators = filter_activators(list_activators);
                    foreach (var activator in lst_activators)
                    {
                        List<EMAIL_ACTIVADORES> list_activadores = da.getActivatorsById(activator.id_activador);
                        if (list_activadores.Count == 1)
                        {
                            EMAIL_ACTIVADORES _activador = list_activadores[0];
                            _activador.procesado = true;

                            bool update_activator = da.updateEmailActivator(_activador);
                            if (!update_activator)
                                LogUtils.InsertarLog(" ERROR - Al procesar el activador " + activator.id_activador + " en EMAIL_ACTIVADORES", true);
                            else
                            {
                                /// 3.1.- Planificar el envío de la campaña
                                Planificar_newsletter plan = new Planificar_newsletter();
                                bool process = plan.process_activators(_activador);
                                if (process)
                                    Console.WriteLine("Activador " + _activador.id_activador + " procesado");
                                else
                                {
                                    EMAIL_ACTIVADORES _act = _activador;
                                    _act.procesado = false;

                                    bool update_activator_filter = da.updateEmailActivator(_act);
                                    if (!update_activator_filter)
                                        LogUtils.InsertarLog(" ERROR - Al procesar el activador " + activator.id_activador + " en EMAIL_ACTIVADORES", true);
                                }
                            }
                        }
                    }
                }

                LogUtils.InsertarLog("Fin del proceso", true);
                Console.WriteLine("Fin del proceso");
            }
            catch (Exception ex)
            {
                LogUtils.InsertarLog(" ERROR - Program.cs::Main()", true);
                LogUtils.InsertarLog("- MSG:" + ex.Message, true);
                LogUtils.InsertarLog("- INNEREXC:" + ((ex.InnerException == null) ? "" : ex.InnerException.Message), true);
            }
        }

        private static List<EMAIL_ACTIVADORES> filter_activators(List<EMAIL_ACTIVADORES> list_activators)
        {
            DataAccess da = new DataAccess();
            List<EMAIL_ACTIVADORES> list = new List<EMAIL_ACTIVADORES>();

            /// 1.- Sacar las campañas
            List<long> lst_id = list_activators.Select(ea => ea.id_camp).Distinct().ToList();

            /// 2.- Recorrer las campañas
            foreach (var id in lst_id)
            {
                List<EMAIL_ACTIVADORES> lst_activators = list_activators.Where(ea => ea.id_camp == id).OrderByDescending(ea => ea.reintento).ToList();
                if (lst_activators.Count == 1)
                    list.Add(lst_activators[0]);
                else
                {
                    List<int> lst_id_type = lst_activators.Select(ea => ea.tipo).Distinct().ToList();
                    foreach (var id_type in lst_id_type)
                    {
                        List<EMAIL_ACTIVADORES> lst_act_type = lst_activators.Where(ea => ea.tipo == id_type).OrderByDescending(ea => ea.reintento).ToList();
                        if (lst_act_type.Count == 1)
                            list.Add(lst_act_type[0]);
                        else
                        {
                            List<EMAIL_ACTIVADORES> lst_activators_filter = da.getActivatorsByIdCampaign(lst_act_type[0].id_camp);
                            if (lst_act_type[0].reintento == 3)
                            {
                                List<EMAIL_ACTIVADORES> lst_act_filter = lst_activators_filter.Where(ea => ea.tipo == id_type && ea.reintento == 2).ToList();
                                if (lst_act_filter.Count == 1)
                                {
                                    if (lst_act_filter[0].procesado == null || (lst_act_filter[0].procesado != null && !lst_act_filter[0].procesado.Value))
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_act_filter[0];
                                        _activador.procesado = true;

                                        bool update_activator = da.updateEmailActivator(_activador);
                                        if (!update_activator)
                                            LogUtils.InsertarLog(" ERROR - Al procesar el activador " + _activador.id_activador + " en EMAIL_ACTIVADORES", true);
                                    }
                                }

                                lst_act_filter = lst_activators_filter.Where(ea => ea.tipo == id_type && ea.reintento == 1).ToList();
                                if (lst_act_filter.Count == 1)
                                {
                                    if (lst_act_filter[0].procesado == null || (lst_act_filter[0].procesado != null && !lst_act_filter[0].procesado.Value))
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_act_filter[0];
                                        _activador.procesado = true;

                                        bool update_activator = da.updateEmailActivator(_activador);
                                        if (!update_activator)
                                            LogUtils.InsertarLog(" ERROR - Al procesar el activador " + _activador.id_activador + " en EMAIL_ACTIVADORES", true);
                                    }
                                }

                                list.Add(lst_act_type[0]);
                            }
                            else if(lst_act_type[0].reintento == 2)
                            {
                                List<EMAIL_ACTIVADORES> lst_act_filter = lst_activators_filter.Where(ea => ea.tipo == id_type && ea.reintento == 1).ToList();
                                if (lst_act_filter.Count == 1)
                                {
                                    if (lst_act_filter[0].procesado == null || (lst_act_filter[0].procesado != null && !lst_act_filter[0].procesado.Value))
                                    {
                                        EMAIL_ACTIVADORES _activador = lst_act_filter[0];
                                        _activador.procesado = true;

                                        bool update_activator = da.updateEmailActivator(_activador);
                                        if (!update_activator)
                                            LogUtils.InsertarLog(" ERROR - Al procesar el activador " + _activador.id_activador + " en EMAIL_ACTIVADORES", true);
                                    }
                                }

                                list.Add(lst_act_type[0]);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
