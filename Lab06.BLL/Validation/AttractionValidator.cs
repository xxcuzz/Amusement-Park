using Lab06.BLL.EntitiesDTO;

namespace Lab06.BLL.Validation
{
    public static class AttractionValidator
    {
        public static bool Validate(ParkAttractionDto item)
        {
            if (item.Price < 0.00M)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                return false;
            }

            return true;
        }
    }
}
