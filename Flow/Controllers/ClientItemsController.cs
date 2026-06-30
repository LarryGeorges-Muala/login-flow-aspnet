using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using OtpNet;
using Flow.Models;

namespace Flow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientItemsController : ControllerBase
    {
        private readonly ClientContext _context;

        public ClientItemsController(ClientContext context)
        {
            _context = context;
        }

        // GET: api/ClientItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientItem>>> GetClientItems()
        {
            return await _context.ClientItems.ToListAsync();
        }

        [HttpGet("session/{email}")]
        public async Task<ActionResult<ClientItem>> GetClientDetails(string email)
        {
            var clientData = _context.ClientItems.FirstOrDefault(u => u.Email == email);
            if (clientData == null)
            {
                return NotFound();
            }

            // var clientItem = await _context.ClientItems.FindAsync(id);

            // if (clientItem == null)
            // {
            //     return NotFound();
            // }

            // return _context.ClientItems.FirstOrDefault(u => u.Email == email);
            
            // Decrypt

            // var encryptionKey = AesProtector.GenerateRandomKey();
            // var test = "aaaaa";
            // Console.WriteLine(eeekey);
            // var eee = AesProtector.Encrypt(test, eeekey);
            // Console.WriteLine(eee);
            // var fff = AesProtector.Decrypt(eee, eeekey);
            // Console.WriteLine(fff);
            // Console.WriteLine("----------------------------");

            // Console.WriteLine("######################################");
            // var encryptionKey = AesProtector.GenerateRandomKey();
            // Console.WriteLine(encryptionKey);
            // Console.WriteLine("######################################");
            // Console.WriteLine(clientData.QrCodeUrl);
            // clientData.QrCodeUrl = AesProtector.Decrypt(clientData.QrCodeUrl, encryptionKey);
            // Console.WriteLine(clientData.QrCodeUrl);

            return clientData;
        }

        [HttpGet("otp/{email}/{code}")]
        public async Task<ActionResult<ClientItem>> CheckMFA(string email, string code)
        {
            var clientData = _context.ClientItems.FirstOrDefault(u => u.Email == email);
            if (clientData == null) {
                return NotFound();
            }

            byte[] secretBytes = Base32Encoding.ToBytes(clientData.Token);
            var totpEvaluator = new Totp(secretBytes);

            // Verification handles minor time drifts natively using verification windows
            bool isValid = totpEvaluator.VerifyTotp(code, out long timeWindowStep);
            if (isValid)
            {
                clientData.Active = true;
                await _context.SaveChangesAsync();
            }

            // var clientItem = await _context.ClientItems.FindAsync(id);

            // if (clientItem == null)
            // {
            //     return NotFound();
            // }

            // return _context.ClientItems.FirstOrDefault(u => u.Email == email);
            
            // Decrypt

            // var encryptionKey = AesProtector.GenerateRandomKey();
            // var test = "aaaaa";
            // Console.WriteLine(eeekey);
            // var eee = AesProtector.Encrypt(test, eeekey);
            // Console.WriteLine(eee);
            // var fff = AesProtector.Decrypt(eee, eeekey);
            // Console.WriteLine(fff);
            // Console.WriteLine("----------------------------");

            // Console.WriteLine("######################################");
            // var encryptionKey = AesProtector.GenerateRandomKey();
            // Console.WriteLine(encryptionKey);
            // Console.WriteLine("######################################");
            // Console.WriteLine(clientData.QrCodeUrl);
            // clientData.QrCodeUrl = AesProtector.Decrypt(clientData.QrCodeUrl, encryptionKey);
            // Console.WriteLine(clientData.QrCodeUrl);

            return clientData;
        }

        // GET: api/ClientItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientItem>> GetClientItem(long id)
        {
            var clientItem = await _context.ClientItems.FindAsync(id);

            if (clientItem == null)
            {
                return NotFound();
            }

            return clientItem;
        }

        // POST: api/ClientItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClientItem>> PostClientItem(ClientItem clientItem)
        {

            // var encryptionKey = AesProtector.GenerateRandomKey();
            // var test = "aaaaa";
            // Console.WriteLine(eeekey);
            // var eee = AesProtector.Encrypt(test, eeekey);
            // Console.WriteLine(eee);
            // var fff = AesProtector.Decrypt(eee, eeekey);
            // Console.WriteLine(fff);
            // Console.WriteLine("----------------------------");

            // Post Data - First Name
            if (clientItem.FirstName == null)
            {
                return Ok(new { Message = "Missing first name...", Completed = false });
            }
            // Post Data - Last Name
            if (clientItem.LastName == null)
            {
                return Ok(new { Message = "Missing last name...", Completed = false });
            }
            // Post Data - Email
            if (clientItem.Email == null)
            {
                return Ok(new { Message = "Missing email...", Completed = false });
            }
            // Post Data - Password
            if (clientItem.Password == null)
            {
                return Ok(new { Message = "Missing password...", Completed = false });
            }
            // Check Existing User
            var clientData = _context.ClientItems.FirstOrDefault(u => u.Email == clientItem.Email);
            if (clientData != null)
            {
                return Ok(new { Message = "Existing account, please login...", Completed = false });
            }

            // Hash Password
            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(clientItem.Password, workFactor: 12);
            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(clientItem.Password, hashedPassword);
            if (isValid) 
            {
                clientItem.Password = hashedPassword;
            }

            // Generate TOTP
            // 160-bit secret key is standard for TOTP
            byte[] secretBytes = KeyGeneration.GenerateRandomKey(20); 
            string secretBase32 = Base32Encoding.ToString(secretBytes);

            // Standardized format that Authenticator apps recognize
            var appIssuer = "ASP.NET Login Flow App";
            string qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString(appIssuer)}:{Uri.EscapeDataString(clientItem.Email)}?secret={secretBase32}&issuer={Uri.EscapeDataString(appIssuer)}&digits=6&period=30";
            // clientItem.Token = secretBase32;
            // clientItem.QrCodeUrl = qrCodeUri;


            // Console.WriteLine("######################################");
            // Console.WriteLine("######################################");
            // Console.WriteLine("######################################");
            // var encryptionKey = AesProtector.GenerateRandomKey();
            // Console.WriteLine(encryptionKey);
            // Console.WriteLine("######################################");
            // Console.WriteLine("######################################");
            // Console.WriteLine("######################################");

            // clientItem.Token = AesProtector.Encrypt(secretBase32, encryptionKey);
            // clientItem.QrCodeUrl = AesProtector.Encrypt(qrCodeUri, encryptionKey);

            clientItem.Token = secretBase32;
            clientItem.QrCodeUrl = qrCodeUri;

            // var eeekey = AesProtector.GenerateRandomKey();
            // var test = "aaaaa";
            // Console.WriteLine(eeekey);
            // var eee = AesProtector.Encrypt(test, eeekey);
            // Console.WriteLine(eee);

            // Create New User
            _context.ClientItems.Add(clientItem);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "New user successfully created...", Completed = true });
        }

        // POST: api/ClientItems/login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("login")]
        public async Task<ActionResult<ClientItem>> LogClientIn(ClientItem clientItem)
        {
            // Post Data - Email
            if (clientItem.Email == null)
            {
                return Ok(new { Message = "Missing email...", Completed = false });
            }
            // Post Data - Password
            if (clientItem.Password == null)
            {
                return Ok(new { Message = "Missing password...", Completed = false });
            }
            // Check Existing User
            var clientData = _context.ClientItems.FirstOrDefault(u => u.Email == clientItem.Email);
            if (clientData == null)
            {
                return Ok(new { Message = "Account not found, please register...", Completed = false });
            }
            // Check Password
            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(clientItem.Password, clientData.Password);
            if (!isValid) 
            {
                return Ok(new { Message = "Invalid password...", Completed = false });
            }
            return Ok(new { Message = "Login successful...", Completed = true });
        }

        // POST: api/ClientItems/update
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<ClientItem>> UpdateClient(ClientItem clientItem)
        {
            // Post Data - First Name
            if (clientItem.FirstName == null)
            {
                return Ok(new { Message = "Missing first name...", Completed = false });
            }
            // Post Data - Last Name
            if (clientItem.LastName == null)
            {
                return Ok(new { Message = "Missing last name...", Completed = false });
            }
            // Post Data - Email
            if (clientItem.Email == null)
            {
                return Ok(new { Message = "Missing email...", Completed = false });
            }
            // Post Data - Password
            if (clientItem.Password == null)
            {
                return Ok(new { Message = "Missing password...", Completed = false });
            }
            // Check Existing User
            var clientData = _context.ClientItems.FirstOrDefault(u => u.Email == clientItem.Email);
            if (clientData == null)
            {
                return Ok(new { Message = "Account not found, please register...", Completed = false });
            }
            // Check Password
            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(clientItem.Password, clientData.Password);
            if (!isValid) 
            {
                return Ok(new { Message = "Invalid password...", Completed = false });
            }
            // Update
            clientData.FirstName = clientItem.FirstName;
            clientData.LastName = clientItem.LastName;
            clientData.Email = clientItem.Email;
            clientData.UserName = clientItem.UserName;
            // Post Data - New Email
            if (clientItem.EmailStateChange != null)
            {
                clientData.Email = clientItem.EmailStateChange;
            }
            // Post Data - New Password
            if (clientItem.PasswordStateChange != null)
            {
                // Hash Password
                string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(clientItem.PasswordStateChange, workFactor: 12);
                bool hashIsValid = BCrypt.Net.BCrypt.EnhancedVerify(clientItem.PasswordStateChange, hashedPassword);
                if (hashIsValid) 
                {
                    clientData.Password = hashedPassword;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Update successful...", Completed = true });
        }

        private bool ClientItemExists(long id)
        {
            return _context.ClientItems.Any(e => e.Id == id);
        }
    }
}
