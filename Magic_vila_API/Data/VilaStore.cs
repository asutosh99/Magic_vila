﻿using Magic_vila_API.Models.DTO;

namespace Magic_vila_API.Data
{
    public static class VilaStore
    {

        public static List<VilaDTO>VilaList=new List<VilaDTO> 
        {
             new VilaDTO { Id = 1,Name="seaView",sqft=1000,occupancy=2323223},
             new VilaDTO { Id = 2,Name="mountainView",sqft=2000,occupancy=99999999},
        };
        
    }
}
