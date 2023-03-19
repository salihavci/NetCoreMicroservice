﻿namespace FreeCourse.Web.Settings
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUri { get; set; }
        public string GatewayBaseUri { get; set; }
        public string PhotoStockUri { get; set; }
        public ServiceAPI Catalog { get; set; }
        public ServiceAPI PhotoStock { get; set; }
        public ServiceAPI Basket { get; set; }
        public ServiceAPI Discount { get; set; }
    }

    public class ServiceAPI
    {
        public string Path { get; set; }
    }
}
