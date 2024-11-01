﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSale.Data.DTO.ResponseModel
{
    public class MeatadataDTO
    {
        public bool hasNextPage { get; set; }
        public bool hasPrevPage { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public int page { get; set; }

        public MeatadataDTO()
        {
        }

        public MeatadataDTO(bool hasNextPage, bool hasPrevPage, int limit, int total, int page)
        {
            this.hasNextPage = hasNextPage;
            this.hasPrevPage = hasPrevPage;
            this.limit = limit;
            this.total = total;
            this.page = page;
        }
    }
}
