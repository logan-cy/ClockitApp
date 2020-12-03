using System.Threading.Tasks;
using CI.SER.DTOs;

namespace CI.SER.Interfaces
{
  public interface IEmail
  {
    Task SendAsync(string emailTo, string body, string subject, EmailOptionsDTO options);
  }
}