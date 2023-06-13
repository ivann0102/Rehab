namespace RehabCV.Extension
{
    public static class DateTimeExtension
    {
        public static int GetAge(this DateTime dateOfBirth)
        {
            if(dateOfBirth.DayOfYear < DateTime.Now.DayOfYear){
                return DateTime.Now.Year - dateOfBirth.Year;
            }
            return DateTime.Now.Year - dateOfBirth.Year - 1;
        }

    }
}