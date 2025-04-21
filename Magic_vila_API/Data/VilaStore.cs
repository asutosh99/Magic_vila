using Magic_vila_API.Models.DTO;

namespace Magic_vila_API.Data
{
    public static class VilaStore
    {

        public static List<VilaDTO>VilaList=new List<VilaDTO> 
        {
             new VilaDTO { Id = 1,Name="seaView",Sqft=1000,Occupancy=2323223},
             new VilaDTO { Id = 2,Name="mountainView",Sqft=2000,Occupancy=99999999},
        };
        
    }
}
