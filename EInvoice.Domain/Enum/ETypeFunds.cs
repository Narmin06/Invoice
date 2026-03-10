namespace EInvoice.Domain.Enum;

public enum ETypeFunds
{
    LoadingUnloading = 1,       // Yükləmə/Boşaldılma
    CommissionBroker = 2,       // Komisyon və broker
    PackagingCosts = 3,         // Qablaşdırma xərcləri
    Insurance = 4,              // Sığorta
    Royalty = 5,                // Royalti
    Licenses = 6,               // Lisenziyalar
    OtherCostsIncrease = 7,     // Digər xərclər (Artıran)
    OtherCostsDecrease = 8      // Digər xərclər (Azaldan)
}