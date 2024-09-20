using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IAddressRepository
    {
        // -- МЕТОДИ ЯКІ ВИКОРИСТОВУЮТЬСЯ НА СТОРІНЦІ ЮЗЕРА В АДРЕСНОМУ АККАРДІОНІ
        Task<IEnumerable<Address>> GetUserAddresses(Guid userId); // отримання списку адрес певного юзера

        Task<Address> GetUserAddressById(Guid? userId, int andressId); // для відкриття форми редагування userId потрібен, а для сторінки з мапою ні тому userId опційний параметр

        Task UpdateAddress(int addressId, Address address); // оновлення інформації одразу в двух таблицях

        Task AddAddressToUser(Guid userId, Address address); // створення запису одразу в двух таблицях

        Task RemoveAddressFromUser(Guid userId, int addressId); // видалення адреси з історії проживання але сама адреса залишається в базі (виделанні зв'язку)

        
        
        
        
        // -- МЕТОДИ ЯКІ ВИКОРИСТОВУЮТЬСЯ НА СТОРІНЦІ АДРЕСИ ТА В АККАРДІОНІ ІСТОРІЇ ПРОЖИВАННЯ

        Task<IEnumerable<Resident>> GetAddressLivingHistory(int andressId); // аккардіон історії проживання на сторінці адреси

        Task AddExistingUserToLivingHistory(Guid userId, int addressId, DateTime moveInDate, DateTime? moveOutDate); // метод для додавання нового проживача в історію проживання певного адресу

        Task TotalDeleteWholeAddress(int addressId); // метод для видалення адреси з бази даних (видалення із бази даних із відповідних таблиць)
    }
}
