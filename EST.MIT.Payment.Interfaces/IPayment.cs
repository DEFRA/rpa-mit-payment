namespace EST.MIT.Payment.Interfaces;

public interface IPayment
{
    void Generate();
    void Store();
    void Send();
}