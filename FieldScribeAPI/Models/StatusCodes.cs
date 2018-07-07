namespace FieldScribeAPI.Models
{
    public static class StatusCodes
    {
        private static int _conflict = 409;

        public static int Status409Conflict
        {
            get
            {
                return _conflict;
            }
        }
    }
}
