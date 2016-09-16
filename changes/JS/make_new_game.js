
var mycurrentscore = 0;
var turn = 0;

$().load(function () {

    turn++;

});

$(document).ready(function () {

    var socket;
    var word = "";
   

      

        socket = new WebSocket("ws://localhost:5000");

        

        socket.addEventListener("open", function (evt) {
            $("#divHistory").append('<h3>Connection Opened with the server.</h3>');
        }, false);

        socket.addEventListener("close", function (evt) {
            $("#divHistory").append('<h3>Connection was closed. :' + evt.reason + '</h3>');
        }, false);

        socket.addEventListener("message", function (evt) {
            $("#divHistory").append('<h3>' + evt.data + '</h3>');
        }, false);

        socket.addEventListener("error", function (evt) {
            alert('Error : ' + evt.message);
        }, false);

     
   //     $("#endButton").click(function () {
  //          socket.close();
   //     });

      
    var main = $("#maingame");

    /*the send button --- must be changed */
    var sbutton = $("<button/>").addClass('submitword');
   
    $(sbutton).attr('id', "sbutton");
    $(sbutton).attr('type', "submit");
    
    main.append($(sbutton).text("SEND"));
    $('.submitword').click(function sub() {
        word ="";
        $('.gcell').each(function (i, obj) {
            
            var chck = $(this).attr('id').substring(2, 3);
            if (chck === 'x')
                word += $(this).html();
        });

          if (socket.readyState === WebSocket.OPEN) {
              socket.send(word);
              
          }
     else {
                $("#divHistory").append('<h3>The underlying connection is closed.</h3>');
    }
   
    });


    /*---end send button----*/


    var ssbutton = $("<button/>").addClass('ssubmitword');

    $(ssbutton).attr('id', "ssbutton");
    $(ssbutton).attr('type', "submit");

    main.append($(ssbutton).text("send_data"));
    $('.ssubmitword').click(function sub() {
        
        if (socket.readyState === WebSocket.OPEN) {
            var user_name = "user1";
            var table_id="";
            var table_data ="";

            $('.gcell').each(function (i, obj) {

                var l = $(this).html();
                var d = $(this).attr('id');
            
                table_id += "[" + d + "]";
                table_data += " : " + l;
            });
            socket.send("i am :" + user_name + " my table data " + table_data +" ++++++ "+ "my table ids :" + table_id);
        }
        
        else {
            $("#divHistory").append('<h3>The underlying connection is closed.</h3>');
        }

    });


    /* score labels --- value must be changed and come from server*/

    var scorelable = $("<lable>").addClass('myscorelable2');

    $(scorelable).attr('id', "myscorelable");

    $('#myscoretablebackround').append($(scorelable).text("0"));
   
    scorelable = $("<lable>").addClass('opscorelable');

    $(scorelable).attr('id', "opscorelable");

    $('#opscoretablebackround').append($(scorelable).text("0"));
    /*----end score labels-----*/

    /*clear all */
    var cancellbutton = $("<button/>").addClass('cancelbutton');

    $(cancellbutton).attr('id', "cancelbutton");

    main.append($(cancellbutton).text("CLEAR"));

    $(".cancelbutton").click(function clear() {

      
        $(':button').each(function (i, obj) {

            var chck = $(this).attr('id').substring(2, 3);
            if (chck === 'x')
                $(this).click();

        });
        word = "";
        $("#divHistory").empty();
     /*----end clear all-----*/
    });

    /* fill the letters --- must be changed and done at server*/
    var array = createArray(5,5);
  
    var a1 = makeid();
   
    var x=5;

    var d1 = a1.split("");
  
    for (var i = 0; i < 5; i++) {
        for (var j = 0; j < 5; j++) {
            array[j][i] = d1[i * x + j];
        }
    }
       
    var data = array;

    /*--end fill letters---*/
                 
    var table = makeTable(main, data);   /*make the board */
    
    

  

    /*cell button functionality */
    $(".gcell").click(function f() {

        mycurrentscore++;

        $('#myscorelable').text(mycurrentscore);
    //   $('#myscoretablebackround').flip();
        /*copy clicked button creation */
        var button = $("<button/>").addClass('ch_gcell'); 
 
      var bid =  button.attr('id', $(this).attr('id')+'x');
        
      
      var bgc = $(this).attr('class');
      button.removeClass($(button).attr('class'));
       button.addClass(bgc);
     
        $(this).attr('value', $(this).html());
        

        var boffset = $(this).offset();
   

       
        $('#wordletters').append(button.text($(this).html()));
        $(bid).css('visibility', 'hidden');

        /*----end copy clicked button--------*/

        /*clicked button animation */
        var cart = $(bid);
      
        var imgtodrag = $(this);
      
        if (imgtodrag) {
            var imgclone = imgtodrag.clone()
                .offset({
                    top: imgtodrag.offset().top,
                    left: imgtodrag.offset().left
                })
                .css({
                    'opacity': '1',
                    'position': 'absolute',
                    'height': '150px',
                    'width': '150px',
                    'z-index': '100'
                })
                .appendTo($('body'))
                .animate({
                    'top': cart.offset().top ,
                    'left': cart.offset().left ,
                    'width': 75,
                    'height': 75
                }, 1000);

            setTimeout(function () {

                imgclone.animate({
                    'width': 0,
                    'height': 0
                }, function () {
                    $(bid).css('visibility', "");
                    $(this).detach()
                });
            }
            )
        }

        /*--end clicked button animation---*/

        button.removeClass($(button).attr('class'));
        button.addClass(bgc);


        $(this).removeClass($(this).attr('class'));
        $(this).addClass('greengcell');
        $(this).unbind("click");
       
        /*----end cell button functionality----- */


        /*copy button functionality */

        button.click(function () {
            mycurrentscore--;
            $('#myscorelable').text(mycurrentscore);
       //     $('#myscoretablebackround').flip();
            var idd = $(this).attr('id').substring(0, 2);
     
            var color = $(this).attr('class');
    
            $('#' + idd).removeClass($('#' + idd).attr('class'));
            $('#' + idd).addClass(color);
            $('#' + idd).on("click",null,f);

            /*copy button animation*/

            var cart = $('#' + idd);

            var imgtodrag = $(this);

            if (imgtodrag) {
                var imgclone = imgtodrag.clone()
                    .offset({
                        top: imgtodrag.offset().top,
                        left: imgtodrag.offset().left
                    })
                    .css({
                        'opacity': '1',
                        'position': 'absolute',
                        'height': '150px',
                        'width': '150px',
                        'z-index': '100'
                    })
                    .appendTo($('body'))
                    .animate({
                        'top': cart.offset().top,
                        'left': cart.offset().left,
                        'width': 75,
                        'height': 75
                    }, 1000);

                setTimeout(function () {

                    imgclone.animate({
                        'width': 0,
                        'height': 0
                    }, function () {
                        $(this).detach()
                    });
                }
                )
            }

            /*--end copy button animation---*/

            $(this).remove();
            
        });
     
        /*--end copy button functionality--*/
    });


  
   
});


function makeTable(container, data) {
    var main = $("#maingame");
    var x = 10;
    var table = $("<table/>").addClass('gtable');
    $.each(data, function (rowIndex, r) {
        var row = $("<tr/>").addClass('grow');
        $.each(r, function (colIndex, c) {
          
            var button = $("<button/>").addClass('gcell');
            button.attr('id', x);
           
            x++;
                row.append($(button).text(c));
                row.append($("<td/>"));
       //     row.append($("<t" + (rowIndex == 0 ? "h" : "d") + "/>").text(c));
        });
        table.append(row);
    });




    return main.append(table);


  
}



function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    for (var i = 0; i < 25; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

function createArray(length) {
    var arr = new Array(length || 0),
        i = length;

    if (arguments.length > 1) {
        var args = Array.prototype.slice.call(arguments, 1);
        while (i--) arr[length - 1 - i] = createArray.apply(this, args);
    }

    return arr;

function send_data() {

        var user_name = "user1";
        var table_data ="";

        $('.ch_gcell').each(function (i, obj) {

            var l = $(this).attr('value');
            
            table_data += " : " + l;
        });
        socket.send("i am :" + user_name + " and my table data: " + table_data);
    }
}

