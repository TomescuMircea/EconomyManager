// See https://aka.ms/new-console-template for more information
using EconomyManager;
using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using EconomyManager.Domain.Utilities;
using EconomyManager.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

/*Console.WriteLine("Hello, World!");*/

Console.WriteLine("If you haven't run the data creation first,\n" +
                  "please run it or register a new user,\n" +
                  "then log into the sytem with it\n\n" +
                  "In order to run the data creation, uncomment the\n" +
                  "\"Create Data\" block located on\n" +
                  "Program.cs then run again the application\n" +
                  "Remember to comment again the code, or it will fail on the next run" +
                  "\n\n\n" +
                  "If you have run the data creation, you will be able to log in\n" +
                  "with any of the random users that have been created. Users are \n" +
                  "identified with email userX@alunos.ipb.pt and password PasswordX \n" +
                  "where X is the user number (starting with 1). For example, you can try\n" +
                  "Email: user1@alunos.ipb.pt\n" +
                  "Password: Passwword1\n" +
                  "The passwords won't be displayed but the program will capture what you type" +
                  "\n\n\n");

/**************************Create Data**************************************/
//RUN ONLY ONCE IF THE DATABASE IS EMPTY

using (var uow = new UnitOfWork())
{
    //Console.WriteLine(uow.);
    var users = EconomyManagerGenerator.generateUsers();
    var categories = EconomyManagerGenerator.generateCategoriesForUserList(users);
    var wallets = EconomyManagerGenerator.generateWalletsForUserList(users);
    var movements = EconomyManagerGenerator.generateMovementsForWalletList(wallets);
    var plans = EconomyManagerGenerator.generatePlansForWalletList(wallets);
    var attachments = EconomyManagerGenerator.generateAttachmentsForMovementList(movements);
    var autoMovements = EconomyManagerGenerator.generateAutomaticMovementsForWalletList(wallets);


    foreach (var user in users)
    {
        Console.WriteLine(user);
        uow.UserRepository.Create(user);
    }
    foreach (var wallet in wallets)
    {
        Console.WriteLine(wallet);
        uow.WalletRepository.Create(wallet);
    }
    foreach( var movement in movements)
    {
        Console.WriteLine(movement);
    }
    foreach (var autoMov in autoMovements)
    {
        Console.WriteLine(autoMov);
    }
    foreach (var category in categories)
    {
        Console.WriteLine(category);
    }
    foreach (var plan in plans)
    {
        Console.WriteLine(plan);
    }
    foreach (var attachmemt in attachments)
    {
        Console.WriteLine(attachmemt);
    }

    await uow.SaveAsync();
}



/**************************MAIN PROGRAM**************************************/



Menu menu = new Menu();

menu.Run();

