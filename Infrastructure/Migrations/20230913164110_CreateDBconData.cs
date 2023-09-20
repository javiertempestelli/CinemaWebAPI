using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDBconData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    GeneroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.GeneroId);
                });

            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    SalaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.SalaId);
                });

            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sonopsis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Trailer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.PeliculaId);
                    table.ForeignKey(
                        name: "FK_Peliculas_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "GeneroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funciones",
                columns: table => new
                {
                    FuncionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Horario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeliculaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funciones", x => x.FuncionId);
                    table.ForeignKey(
                        name: "FK_Funciones_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "PeliculaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funciones_Salas_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Salas",
                        principalColumn: "SalaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FuncionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Funciones_FuncionId",
                        column: x => x.FuncionId,
                        principalTable: "Funciones",
                        principalColumn: "FuncionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funciones_PeliculaId",
                table: "Funciones",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Funciones_SalaId",
                table: "Funciones",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Peliculas_GeneroId",
                table: "Peliculas",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FuncionId",
                table: "Tickets",
                column: "FuncionId");

            // Se crean los Generos de las peliculas
            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Acción" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Aventuras" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Ciencia Ficcion" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Comedia" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Documental" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Drama" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Fantasía" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Musical" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Suspenso" });


            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Nombre" },
                values: new object[] { "Terror" });

            // Se crean las 3 Salas en el Cine
            migrationBuilder.InsertData(
                table: "Salas",
                columns: new[] { "Nombre", "Capacidad" },
                values: new object[] { "Sala 1", 5 });

            migrationBuilder.InsertData(
                table: "Salas",
                columns: new[] { "Nombre", "Capacidad" },
                values: new object[] { "Sala 2", 10 });

            migrationBuilder.InsertData(
                table: "Salas",
                columns: new[] { "Nombre", "Capacidad" },
                values: new object[] { "Sala 3", 35 });

            // Se insertan las 20 peliculas 

            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Megalodón 2: La fosa",
                                        "Después de detectar un aumento de actividad en una fosa marina de gran profundidad, Jonas Taylor (Jason Statham) y su equipo tendrán que enfrentarse a un depredador ancestral",
                                        "https://es.web.img2.acsta.net/c_310_420/pictures/23/05/09/10/42/0347977.jpg",
                                        "https://youtu.be/7wuK5PhzcNY?si=iH8OrEpFTkDMamrw" ,
                                        "1"});

            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Gran Turismo",
                                        "Basada en la historia real de Jann Mardenborough, un chaval aficionado a los videojuegos de carreras que sueña con ser piloto profesional y que empieza a entrenar para aprender a conducir en un circuito donde competirá para cumplir su sueño.",
                                        "https://es.web.img2.acsta.net/c_310_420/pictures/23/05/03/14/13/0249485.jpg",
                                        "https://youtu.be/f5IRCGKZgJY?si=Q9UWEFByyTuBljqc" ,
                                        "1"});
            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Harry Potter y la piedra filosofal",
                                        "El día en que cumple once años, Harry Potter se entera de que es hijo de dos destacados hechiceros, de los que ha heredado poderes mágicos. En la escuela Hogwarts de Magia y Hechicería",
                                        "https://pics.filmaffinity.com/harry_potter_and_the_sorcerer_s_stone-154820574-mmed.jpg",
                                        "https://youtu.be/ZgrCZVjPg9g?si=i5faNObjPjxipKeU" ,
                                        "2"});
            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Jurassic Park",
                                        "John Hammond consigue hacer realidad su sueño de clonar dinosaurios y crear con ellos un parque temático en una isla remota.",
                                        "https://pics.filmaffinity.com/jurassic_park-187298880-mmed.jpg",
                                        "https://youtu.be/dLDkNge_AhE?si=7ni1abXeAzJ_dMnO" ,
                                        "2"});
            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Volver al futuro",
                                        "El adolescente Marty McFly es amigo de Doc, un científico al que todos toman por loco. Cuando Doc crea una máquina para viajar en el tiempo, un error fortuito hace que Marty llegue a 1955. ",
                                        "https://pics.filmaffinity.com/back_to_the_future-100822308-mmed.jpg",
                                        "https://youtu.be/GM6_MHRc4Xo?si=ksmsX9To6ChUw1yo" ,
                                        "3"});
            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Matrix",
                                        "Thomas Anderson es un brillante programador de una respetable compañía de software. Pero fuera del trabajo es Neo, un hacker que un día recibe una misteriosa visita",
                                        "https://pics.filmaffinity.com/the_matrix-155050517-mmed.jpg",
                                        "https://youtu.be/m8e-FF8MsqU?si=HLIp75IyoJXYkZSQ" ,
                                        "3"});
            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
                values: new object[] {  "Un Novio para mi Mujer",
                                        "Un hombre contrata a un seductor profesional para que seduzca a su esposa y la haga pedirle el divorcio.",
                                        "https://pics.filmaffinity.com/un_novio_para_mi_mujer-757863751-mmed.jpg",
                                        "https://youtu.be/l0S_CtrkgnU?si=S7qn-gK6ibyWxxdG" ,
                                        "4"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Déjala correr",
                                        "Un repartidor de pizzas veinteañero organiza una cena en su casa para conquistar a una chica algo mayor que él. El protagonista trata de arreglar cuando descubre que puede manipular el tiempo mediante el botón de rebobinado de una videocámara.",
                                        "https://pics.filmaffinity.com/dejala_correr-164728813-mmed.jpg",
                                        "https://youtu.be/R3NUakdiIhM?si=n-GZ-VSF0RBTzMAm" ,
                                        "4"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Favio: Crónica de un Director",
                                        "Un retrato íntimo de Leonardo Favio, un legendario director argentino, y su influencia en la cultura cinematográfica.",
                                        "https://pics.filmaffinity.com/favio_cronica_de_un_director-437828549-mmed.jpg",
                                        "https://youtu.be/-4LudLKf7cY?si=esjZt77H2Khl4LXT" ,
                                        "5"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Héroes: Silencio y Rock & Roll",
                                        "Documental que narra la historia de Héroes del Silencio desde la creación de la banda. Está contada en primera persona, y a través de ingente material videográfico y fotográfico, nunca visto antes",
                                        "https://pics.filmaffinity.com/heroes_silencio_y_rock_roll-172242554-mmed.jpg",
                                        "https://youtu.be/ZWEV4Tcvwmw?si=ne1qfeVsqzlRCKna" ,
                                        "5"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "El Hijo de la Novia",
                                        "Un restaurante familiar enfrenta crisis mientras el protagonista lucha por reconciliarse con su madre y su pasado.",
                                        "https://pics.filmaffinity.com/el_hijo_de_la_novia-570644260-mmed.jpg",
                                        "https://youtu.be/AoAU_eeSq4o?si=vj42P30ZTvxmxaV9" ,
                                        "6"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "El discurso del Rey",
                                        "El duque de York se convirtió en rey de Inglaterra con el nombre de Jorge VI (1936-1952), tras la abdicación de su hermano mayor, Eduardo VIII. Su tartamudez, que constituía un gran inconveniente para el ejercicio de sus funciones.",
                                        "https://pics.filmaffinity.com/the_king_s_speech-997653906-mmed.jpg",
                                        "https://youtu.be/KpssjoKZK1w?si=OdUMh27tJOM6cL3q" ,
                                        "6"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Dragon Ball Z: La resurrección de Freezer",
                                        "Después de que Bills, decidiera no destruir la Tierra, se vive una gran época de paz. Hasta que, antiguos miembros de élite de Freezer, llegan a la Tierra con el objetivo de revivir a su líder",
                                        "https://pics.filmaffinity.com/dragon_ball_z_fukkatsu_no_f-911759024-mmed.jpg",
                                        "https://youtu.be/cD8Vv5bdmbI?si=xa3rXfyyfXcid6Yp" ,
                                        "7"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Dragon Ball Z: La batalla de los dioses",
                                        "lgunos años después de la batalla con Majin Buu, Bils, el dios de la destrucción, encargado de mantener el equilibrio del universo, se ha despertado de un largo sueño. Bils parte a la búsqueda de Goku. ",
                                        "https://es.web.img3.acsta.net/pictures/14/03/13/09/32/445665.jpg",
                                        "https://youtu.be/pn0_qnfsrfc?si=ZU2l7eV2ZHKISQ1L" ,
                                        "7"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "La La Land",
                                        "Una joven aspirante a actriz que trabaja como camarera mientras acude a castings, y Sebastian, un pianista de jazz que se gana la vida tocando en sórdidos tugurios, se enamoran.",
                                        "https://pics.filmaffinity.com/la_la_land-262021831-mmed.jpg",
                                        "https://youtu.be/0pdqf4P9MB8?si=zic1kY_1HN3zVE28" ,
                                        "8"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Esperando la Carroza",
                                        "Una comedia musical que sigue las desventuras de una familia disfuncional durante el funeral de la abuela.",
                                        "https://pics.filmaffinity.com/esperando_la_carroza-675954190-mmed.jpg",
                                        "https://youtu.be/KIgOuEPQZsU?si=UUb2Wru_MiFzUMeT" ,
                                        "8"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "El Método",
                                        "Un grupo de aspirantes a ejecutivos compite brutalmente por un codiciado puesto de trabajo en una entrevista de selección.",
                                        "https://pics.filmaffinity.com/el_metodo-411717926-mmed.jpg",
                                        "https://youtu.be/kkqpiImZ5ko?si=uCOWJrypBDUHji2D" ,
                                        "9"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Relaciones Peligrosas",
                                        "Un psicólogo se ve involucrado en un peligroso juego de seducción con una enigmática paciente.",
                                        "https://pics.filmaffinity.com/open_marriage_to_have_and_to_kill-372578448-mmed.jpg",
                                        "https://youtu.be/OQFGYEVsLQU?si=e-lvJDqYuTr7owOY" ,
                                        "9"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "Terrified",
                                        "Una serie de eventos aterradores afecta a una comunidad, y un equipo de investigadores busca respuestas sobrenaturales.",
                                        "https://pics.filmaffinity.com/aterrados-800169699-mmed.jpg",
                                        "https://youtu.be/oUmw0tRytDc?si=pUYyYyoXaFmMNTNq" ,
                                        "10"});
            migrationBuilder.InsertData(
               table: "Peliculas",
               columns: new[] { "Titulo", "Sonopsis", "Poster", "Trailer", "GeneroId" },
               values: new object[] {  "El Espinazo del Diablo",
                                        "En un orfanato durante la Guerra Civil Española, un niño se enfrenta a sucesos paranormales y oscuros secretos.",
                                        "https://es.web.img2.acsta.net/c_310_420/medias/nmedia/18/82/09/38/19598918.jpg",
                                        "https://youtu.be/c3gRqYu5olY?si=21jn-0njwEEt692a" ,
                                        "10"});
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Funciones");

            migrationBuilder.DropTable(
                name: "Peliculas");

            migrationBuilder.DropTable(
                name: "Salas");

            migrationBuilder.DropTable(
                name: "Generos");
        }
    }
}
