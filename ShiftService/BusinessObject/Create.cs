namespace ShiftService
{
    public class Create
    {
        public bool CreateService(Service oServie)
        {
            Dictionary<string, dynamic> lstParameters = new Dictionary<string, dynamic>()
            {
                { "cod_tipo_serv",oServie.ServiceType },
                { "estado",oServie.Status },
                { "valor",oServie.Value },
                { "direccion",oServie.Address },
                { "fecha_hora_inicio",oServie.startDate.ToString("yyyy-MM-dd HH:mm:ss")},
                { "fecha_hora_fin",oServie.EndDate.ToString("yyyy-MM-dd HH:mm:ss") },
                { "usuario_creacion","2" },
                { "fecha_creacion",oServie.DatetimeCreation.ToString("yyyy-MM-dd HH:mm:ss") }
            };
           
            return MySqlStartup.CallStoredProcedure_Update(lstParameters, "CreateService");

        }
    }
}
