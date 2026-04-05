using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// 🔑 ВСТАВЬ СВОЙ ТОКЕН ОТ @BotFather
var botClient = new TelegramBotClient("8656786841:AAEPtimbZDE0Jj2wgssMPaPLyVWH-HdzUxQ");

using var cts = new CancellationTokenSource();

botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() },
    cancellationToken: cts.Token
);

Console.WriteLine("🤖 Бот запущен. Нажмите Enter для остановки...");
await Task.Delay(-1, cts.Token);

async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message) return;
    if (message.Text is not { } messageText) return;

    var chatId = message.Chat.Id;
    Console.WriteLine($"📨 Получено: {messageText} от {chatId}");

    if (messageText.StartsWith("/"))
    {
        await HandleCommand(bot, chatId, messageText, cancellationToken);
    }
    else
    {
        await bot.SendMessage(chatId, "Используйте команды: /start, /help", cancellationToken: cancellationToken);
    }
}

async Task HandleCommand(ITelegramBotClient bot, long chatId, string command, CancellationToken cancellationToken)
{
    var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var cmd = parts[0].ToLower();

    switch (cmd)
    {
        case "/start":
            await bot.SendMessage(chatId,
                "🤖 Привет! Я демо-бот.\n\n" +
                "Доступные команды:\n" +
                "/help — показать список команд\n" +
                "/about — информация о проекте",
                cancellationToken: cancellationToken);
            break;

        case "/help":
            await bot.SendMessage(chatId,
                "Доступные команды:\n" +
                "/start — приветствие\n" +
                "/help — эта справка\n" +
                "/about — информация о проекте",
                cancellationToken: cancellationToken);
            break;

        case "/about":
            await bot.SendMessage(chatId,
                "📌 Todo Telegram Bot\n\n" +
                "Версия: 1.0 (Demo)\n" +
                "Технологии: C#, .NET 10, Telegram.Bot API\n" +
                "Автор: Намазов Сохбат\n\n" +
                "Полная версия с интеграцией с Todo API будет доступна позже.",
                cancellationToken: cancellationToken);
            break;

        default:
            await bot.SendMessage(chatId, "Неизвестная команда. /help — список команд", cancellationToken: cancellationToken);
            break;
    }
}

Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine($"❌ Ошибка: {exception.Message}");
    return Task.CompletedTask;
}