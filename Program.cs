using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var chapters = new List<(string Title, string Content)>
{
    (
        "Chapter 1 - The Frozen Yard",
        """
        Pyrix awoke beneath a dead oak tree in Ghost Yard.

        Snow drifted from a sky without stars.

        A black crow watched from above.

        "You're late," the crow said.

        Pyrix stepped back.

        "Late for what?"

        The crow spread its wings.

        "The end."
        """
    ),

    (
        "Chapter 2 - Whispering Frost",
        """
        Every frozen house contained shadows.

        The shadows whispered memories.

        As Pyrix walked through the village, a voice spoke:

        'Don't let the cold take your name.'

        Somewhere beyond Ghost Yard,
        something ancient was waking.
        """
    ),

    (
        "Chapter 3 - The Hollow Bell",
        """
        Beneath the cemetery stood an ancient bell tower.

        Every midnight the bell rang.

        No rope.

        No wind.

        Yet the sound echoed across the valley.

        The crow refused to enter.
        """
    ),

    (
        "Chapter 4 - The Nameless King",
        """
        The stories were true.

        Beneath the frozen earth slept
        the Nameless King.

        He fed on forgotten memories.

        Every winter he grew stronger.
        """
    ),

    (
        "Chapter 5 - The Crow's Secret",
        """
        The crow revealed its true name.

        Mournwing.

        For centuries it guarded the prison
        beneath Ghost Yard.

        The cold was not weather.

        It was the breath of the king.
        """
    ),

    (
        "Chapter 6 - The Ice Caves",
        """
        Blue crystal caves stretched deep beneath
        the mountains.

        Inside every crystal floated a memory.

        Entire lives had been stolen.
        """
    ),

    (
        "Chapter 7 - The Heart Light",
        """
        At the center of the cave stood
        an ancient lantern.

        Its flame never died.

        Mournwing called it the Heart Light.

        Only the Heart Light could stop
        the Nameless King.
        """
    ),

    (
        "Chapter 8 - The Battle",
        """
        The frozen throne cracked.

        Ice exploded across the cavern.

        The Nameless King opened his eyes.

        Darkness poured through the chamber.

        The final battle had begun.
        """
    ),

    (
        "Chapter 9 - Sacrifice",
        """
        The lantern demanded a price.

        A memory.

        Not just any memory.

        The memory Pyrix treasured most.

        To save the world,
        he would lose part of himself forever.
        """
    ),

    (
        "Chapter 10 - Dawn Over Ghost Yard",
        """
        The darkness faded.

        Snow began to melt.

        Sunlight returned for the first time
        in generations.

        The people of Ghost Yard were saved.

        Yet nobody remembered the hero
        who rescued them.

        Pyrix smiled anyway.
        """
    )
};

app.MapGet("/", () =>
{
    var html = """
    <html>
    <head>
        <title>Cold Nightmare Dreams</title>
        <style>
            body{
                background:#0d1117;
                color:white;
                font-family:Arial;
                max-width:900px;
                margin:auto;
                padding:30px;
            }
            a{
                color:#58a6ff;
                display:block;
                margin:10px 0;
            }
        </style>
    </head>
    <body>
        <h1>Cold Nightmare Dreams</h1>
        <p>Interactive Horror Fantasy Story</p>
    """;

    for (int i = 0; i < chapters.Count; i++)
    {
        html += $"<a href='/chapter/{i}'>{chapters[i].Title}</a>";
    }

    html += "</body></html>";

    return Results.Content(html, "text/html");
});

app.MapGet("/chapter/{id:int}", (int id) =>
{
    if (id < 0 || id >= chapters.Count)
        return Results.NotFound();

    var chapter = chapters[id];

    var prev = id > 0
        ? $"<a href='/chapter/{id - 1}'>Previous</a>"
        : "";

    var next = id < chapters.Count - 1
        ? $"<a href='/chapter/{id + 1}'>Next</a>"
        : "";

    return Results.Content($"""
    <html>
    <head>
        <title>{chapter.Title}</title>
        <style>
            body {{
                background:#0d1117;
                color:white;
                font-family:Arial;
                max-width:900px;
                margin:auto;
                padding:30px;
            }}

            .nav {{
                margin-top:20px;
            }}

            a {{
                color:#58a6ff;
                margin-right:20px;
            }}
        </style>
    </head>
    <body>
        <h1>{chapter.Title}</h1>

        <pre style="white-space:pre-wrap;font-size:18px;">
{chapter.Content}
        </pre>

        <div class="nav">
            {prev}
            {next}
            <a href="/">Contents</a>
        </div>
    </body>
    </html>
    """, "text/html");
});

app.Run();
