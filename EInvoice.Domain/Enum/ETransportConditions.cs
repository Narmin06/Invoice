namespace EInvoice.Domain.Enum;

public enum ETransportConditions
{
    EXW = 1,      // Zavoddan
    FCA = 2,      // Franko daşıyıcı
    FAS = 3,      // Gəmi boyunca sərbəst
    FOB = 4,      // Gəmi içərisində sərbəst
    CFR = 5,      // Dəyər və icarə
    CIF = 6,      // Dəyər, sığortalaşdırma və icarə
    CPT = 7,      // Daşınma ... qədər ödənilmişdir
    CIP = 8,      // Daşınma və sığortalaşdırma ... qədər ödənilmişdir
    DAT = 9,      // Terminala qədər çatdırma
    DAP = 10,     // Məntəqəyə qədər çatdırma
    DDP = 11      // Rüsumları ödənilmiş göndəriş
}