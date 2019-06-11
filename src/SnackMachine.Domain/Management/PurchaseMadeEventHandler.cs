namespace SnackMachine.Domain
{
    public class PurchaseMadeEventHandler : IHandler<PurchaseMadeEvent>
    {
        private readonly HeadOfficeRepository _headOfficeRepository;

        public PurchaseMadeEventHandler(HeadOfficeRepository headOfficeRepository)
        {
            _headOfficeRepository = headOfficeRepository;
        }

        public void Handle(PurchaseMadeEvent domainEvent)
        {
            var headOffice = HeadOfficeInstance.Instance;
            if(headOffice != null)
            {
                headOffice.ChangeSalesAmount(domainEvent.Amount);
                _headOfficeRepository.Save(headOffice);
            }
        }
    }
}