namespace M.YZ.Basement.Utilities.Services.MessageBus;
public interface IMessageConsumer
{
    void ConsumeEvent(string sender, Parcel parcel);
    void ConsumeCommand(string sender, Parcel parcel);
}
