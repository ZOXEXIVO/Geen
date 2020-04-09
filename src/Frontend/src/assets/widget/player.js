var widget = function(){ 
    createFont();   
    createStyle();
    
    var className = 'geenw';
    var dataUrl =  'https://geen.one/api/player/random';

    var elements = document.getElementsByClassName(className);

    for (index = 0; index < elements.length; index++) {
        var element = elements[index];

        element.style.width = '22em';
        element.style.height = '5em';
        element.style.border = "1px solid #bfbfbf";
        element.style.boxShadow = "0 0 7px 1px #a2abaa;";

        upd(element);   
    }

    function upd(elem){
        geen_request(dataUrl, function(data){
            elem.innerHTML = createHtmlForPlayer(data); 
        }); 
    };

    function createFont(){
        style = document.createElement('link'); 

        style.rel = 'stylesheet'; 

        style.href = 'https://fonts.googleapis.com/css?family=Play'; 

        document.head.appendChild(style);
    }       
    
    function createHtmlForPlayer(player){
        return `<div style="float:left;"><a href="https://geen.one/player/` + player.urlName + `?source=widget" target="blank">
        <img src="https://storage.geen.one/geen/` + player.id + `.jpg" class="geenw-thumb" style="width:58px;height:58px;margin: 12px 0 0 11px;">
      </a></img></div><div style="float:left;margin-left: 20px;margin-top:8px;min-width: 15em">
        <a href="https://geen.one?source=widget"><img src="https://geen.one/assets/images/logo.svg?v=2.0" style="float:right;width: 70px;height: 45px;margin: 8px 0 0 11px;" /></a>
        <p style="font-size: 19px; font-weight: bold;margin: 0;">
            <a href="https://geen.one/player/` + player.urlName + `?source=widget" style="color:#5f5f5f;font-weight: bold;text-decoration:none" target="blank">
            ` + player.lastName + `
            </a>
        </p>
        <p style="font-size: 15px; font-weight: bold;margin: 0;">
            <a href="https://geen.one/player/` + player.urlName + `?source=widget" style="color:#5f5f5f;font-weight: bold;text-decoration:none" target="blank">
            ` + (player.firstName || '') + `
            </a>        
        </p>
        <p style="text-transform: uppercase;margin: 7px 0 0 0; font-size: 13px;">
          <a href="https://geen.one/club/` + player.club.urlName + `?source=widget" style="color:#009090;font-weight: bold;text-decoration:none" target="blank">
          ` + (player.club.name) + `
          </a>
        </p>      
      </div>`;
    } 

    function createStyle(){
        var script = {
            type: 'text/css', style: document.createElement('style'), 
            content: `.geenw,
            .geenw .geenw-thumb { 
              line-height: 1.12857143 !important;
              color:#5f5f5f;
              font-family: 'Play';    
              -webkit-box-shadow: 0px 0px 7px 1px rgba(162, 171, 170, 1);
              -moz-box-shadow: 0px 0px 7px 1px rgba(162, 171, 170, 1);
               box-shadow: 0px 0px 7px 1px rgba(162, 171, 170, 1);
               border-radius: 5px;
            }            
            .geenw a {
                text-decoration: none !important;
                border: none;
            }            
            `,
            append: function() {
          
              this.style.type = this.type;
              this.style.appendChild(document.createTextNode(this.content));
              document.head.appendChild(this.style);
          
          }}; 
          
          script.append();
    }    

    function geen_request(url, callback) {
        var xmlhttp = new XMLHttpRequest();
    
        xmlhttp.onreadystatechange = function() {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                try {
                    var data = JSON.parse(xmlhttp.responseText);
                } catch(err) {
                    console.log(err.message + " in " + xmlhttp.responseText);
                    return;
                }
                callback(data);
            }
        };
     
        xmlhttp.open("GET", url, true);
        xmlhttp.send();
    }
}(); 