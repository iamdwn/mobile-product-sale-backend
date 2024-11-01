using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSale.Data.DTO.ResponseModel
{
    public class ResponseDTO
    {
        public Object Content { get; set; }
        public string Message { get; set; }
        public List<String> Details { get; set; }
        public int StatusCode { get; set; }
        public MeatadataDTO MeatadataDTO { get; set; }
    }
}
