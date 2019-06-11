namespace SnackMachine.Domain
{
    public static class HeadOfficeInstance
    {
        private const long HeadOfficeId = 1;
        public static HeadOffice Instance { get; private set; }

        public static void Init(SnackMachineContext context)
        {
            if(Instance == null)
            {
                var repo = new HeadOfficeRepository(context);
                Instance = repo.GetById(HeadOfficeId);
            }
        }
    }
}