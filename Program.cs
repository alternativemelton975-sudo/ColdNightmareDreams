using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var chapters = new List<(string Title, string Content)>
{
    (
        "Chapter 1 - The Frozen Yard",
        @"Pyrix awoke beneath a dead oak tree in Ghost Yard.

Snow drifted from a sky without stars.

A black crow watched from above.

""You're late,"" the crow said.

Pyrix stepped back.

""Late for what?""

The crow spread its wings.

""The end."""
    ),

    (
        "Chapter 2 - Whispering Frost",
        @"Every frozen house contained shadows.

The shadows whispered memories.

As Pyrix walked through the village, a voice spoke:

'Don't let the cold take your name.'

Somewhere beyond Ghost Yard,
something ancient was waking."
    ),

    (
        "Chapter 3 - The Hollow Bell",
        @"Deep beneath the cemetery stood a rusted bell tower.

No rope swung from the ancient bell.

No wind touched its surface.

Yet every midnight it rang.

The crow refused to enter the tower.

Pyrix climbed alone."
    ),

    (
        "Chapter 4 - The Nameless King",
        @"The stories were true.

Beneath the frozen earth slept
the Nameless King.

He fed upon forgotten memories.

Every winter he grew stronger."
    ),

    (
        "Chapter 5 - The Crow's Secret",
        @"The crow finally revealed its true name.

Mournwing.

For centuries it guarded the prison
beneath Ghost Yard.

The cold was not weather.

It was the breath of the king."
    ),

    (
        "Chapter 6 - The Crystal Caves",
        @"The caves glowed blue beneath the mountains.

Every crystal held a memory.

Entire lives were trapped there.

Families.

Friends.

Dreams."
    ),

    (
        "Chapter 7 - The Heart Light",
        @"At the center of the caves stood a lantern.

Its flame had burned for centuries.

Only the Heart Light could weaken
the Nameless King.

But every use required sacrifice."
    ),

    (
        "Chapter 8 - Frozen Giants",
        @"Ancient giants slept beneath the ice.

When the king stirred,
so did they.

Entire mountains began to move.

Ghost Yard trembled."
    ),

    (
        "Chapter 9 - The Memory Gate",
        @"A gate of silver frost appeared.

Beyond it lay every stolen memory.

Pyrix walked carefully.

Thousands of voices called his name."
    ),

    (
        "Chapter 10 - The Battle Begins",
        @"The frozen throne cracked.

Ice exploded through the cavern.

The Nameless King opened his eyes.

Darkness poured out like smoke.

The final battle had begun."
    ),

    (
        "Chapter 11 - The Sacrifice",
        @"The Heart Light demanded payment.

Not gold.

Not blood.

A precious memory.

One Pyrix would never recover."
    ),

    (
        "Chapter 12 - Dawn Over Ghost Yard",
        @"The darkness faded.

The snow began to melt.

Sunlight returned.

The people celebrated.

Yet nobody remembered the hero
who saved them.

Pyrix smiled anyway."
    )
};

app.MapGet("/", () =>
{
    string html = @"
    <html>
    <head>
        <title>Cold Nightmare Dreams</title>
        <style>
            body {
                background:#0d1117;
                color:white;
                font-family:Arial, sans-serif;
                max-width:900px;
                margin:auto;
                padding:30px;
            }

            h1 {
                color:#58a6ff;
            }

            a {
                display:block;
                color:#58a6ff;
                margin:10px 0;
                text-decoration:none;
            }

            a:hover {
                text-decoration:underline;
            }
        </style>
    </head>
    <body>
        <h1>Cold Nightmare Dreams</h1>
        <p>A dark fantasy adventure following Pyrix and Mournwing through Ghost Yard.</p>";

    for (int i = 0; i < chapters.Count; i++)
    {
        html += $"<a href='/chapter/{i}'>" +
                $"{chapters[i].Title}</a>";
    }

    html += @"
    </body>
    </html>";

    return Results.Content(html, "text/html");
});

app.MapGet("/chapter/{id:int}", (int id) =>
{
    if (id < 0 || id >= chapters.Count)
        return Results.NotFound();

    var chapter = chapters[id];

    string prevLink = "";
    string nextLink = "";

    if (id > 0)
        prevLink = $"<a href='/chapter/{id - 1}'>⬅ Previous</a>";

    if (id < chapters.Count - 1)
        nextLink = $"<a href='/chapter/{id + 1}'>Next ➡</a>";

    string html = $@"
    <html>
    <head>
        <title>{chapter.Title}</title>
        <style>
            body {{
                background:#0d1117;
                color:white;
                font-family:Arial, sans-serif;
                max-width:900px;
                margin:auto;
                padding:30px;
            }}

            h1 {{
                color:#58a6ff;
            }}

            a {{
                color:#58a6ff;
                margin-right:20px;
                text-decoration:none;
            }}

            pre {{
                white-space:pre-wrap;
                font-size:18px;
                line-height:1.6;
            }}
        </style>
    </head>
    <body>

        <h1>{chapter.Title}</h1>

        <pre>{chapter.Content}</pre>

        <hr>

        {prevLink}
        {nextLink}
        <a href='/'>Contents</a>

    </body>
    </html>";

    return Results.Content(html, "text/html");
});

app.Run();
