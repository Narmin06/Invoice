namespace EInvoice.Domain.Enum;

public enum EPaymentConditions
{
                            // BUY 
    Payment100 = 100,       // Malın dəyəri əvvəlcədən ödənilmişdir
    Payment110 = 110,       // Malın dəyərinin bir hissəsi əvvəlcədən ödənilmişdir
    Payment120 = 120,       // Malın dəyəri idxaldan sonra ödəniləcək
    Payment130 = 130,       // Mal xarici kredit hesabına idxal olunduqda
    Payment140 = 140,       // İdxal olunan malın dəyərinin ödənişi nəzərdə tutulmadıqda
    Payment200 = 200,       // Malın dəyəri xarici kredit hesabına əvvəlcədən ödənilmişdir
    Payment210 = 210,       // Malın dəyərinin bir hissəsi xarici kredit hesabına ödənilmişdir
    Payment220 = 220,       // Konsiqnasiya şərti ilə idxal olunmuş mal

                            // SELL
    Payment001 = 1,         // Malın dəyəri əvvəlcədən ödənilmişdir (ixracatçının hesabına)
    Payment002 = 2,         // Mal ixrac olunduqdan sonra ödənilmişdir
    Payment003 = 3,         // Konsiqnasiya yolu ilə mal ixracı zamanı ödənilmişdir
    Payment004 = 4,         // Malın dəyərinin bir hissəsi əvvəlcədən, qalan hissəsi isə ixracdan sonra ödənildikdə
    Payment005 = 5,         // Hasilatın pay bölgüsü ilə ödənilməsi
    Payment006 = 6,         // Hasilatın pay bölgüsü ilə ödənilmədən ixrac edildikdə
}