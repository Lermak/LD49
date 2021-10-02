(async () => 
{
  //For web debugging
  if(typeof CefSharp === "undefined") {
    CefSharp = {}
    CefSharp.BindObjectAsync = () => {}

    game = {}
    game.readFile = (path) => {
      return `[ "Hello!" ]`
    }
  }

  await CefSharp.BindObjectAsync("game", "bound");


  let People = []
  

  function addPerson(name, icon) {
    let person = {
      name: name,
      displayName: name,
      icon: "images/" + icon + ".png",
      responses: [],
      messages: []
    }

    game.readFile(`Content/Web/Chat/people/${name}/responses.json`).then((r) => {
      if(r != "") {
        person.responses = JSON.parse(r)
      }
    })

    People.push(person)
    return person
  }

  function addMessage(personMessage, fromPerson, message, time) {
    if(typeof personMessage === "string") {
      personMessage = People.find((p) => {return p.name == personMessage} )
    }

    if(typeof fromPerson === "string") {
      fromPerson = People.find((p) => {return p.name == fromPerson} )
    }

    let msg = {
      from: fromPerson,
      text: message,
      time: time
    }
    personMessage.messages.push(msg)
    return msg
  }

  let you = addPerson("Jogn Idogun", "default_icon");
  you.displayName = "Jogn Idogun (You)"

  addPerson("Delores", "default_icon")
  addPerson("Sally", "default_icon")
  addPerson("Joe", "default_icon")
  addPerson("Mary", "default_icon")

  addMessage("Sally", "Sally", "Hello!", "now")


  let SelectedPerson = People[0]

  /* For later

    <div class="quoted-file">
      <div class="preparatory-text">
        <p>Post</p>
        <i class="fas fa-sort-down"></i>
      </div>
      <div class="file-figure">
        <i class="far fa-file-alt fa-2x"></i>
        <div class="file-detail">
          <h5>1/9 Meeting Notes</h5>
          <small>Last edited just now</small>
        </div>
      </div>
    </div>



    <div class="quoted">
      <h5>Team status meeting</h5>
      <p class="quoted-text">Today from 1:00PM to 1:30PM</p>
    </div>


    <span class="mention">@lisa</span>
    */


  function createMessageHTML(fromPerson, message, time) {
    time = "10:00pm";

    return `
    <article class="feed">
      <section class="feeds-user-avatar">
        <img src="${fromPerson.icon}" alt="${fromPerson.name}" width="40" />
      </section>
      <section class="feed-content">
        <section class="feed-user-info">
          <h4>${fromPerson.name} <span class="time-stamp">${time}</span></h4>
        </section>
        <div>
          <p class="feed-text">
            ${message}
          </p>
        </div>
      </section>
    </article>
    `
  }

  function sendMessage(text) {
    let mainfeed = document.getElementById("mainfeed")

    let toPerson = SelectedPerson
    let msg = addMessage(toPerson, you, text, "now")
    mainfeed.innerHTML += createMessageHTML(msg.from, msg.text, msg.time)

    if(toPerson.responses.length > 0) {
      let response_msg = addMessage(toPerson, toPerson, toPerson.responses[0], "now")
      mainfeed.innerHTML += createMessageHTML(response_msg.from, response_msg.text, response_msg.time)
    }

    mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
  }

  window.switchToChat = (id) => {
    let person = People[id]
    SelectedPerson = person

    let header = document.getElementById("channel-name")
    header.innerHTML = person.displayName

    let header_avatar = document.getElementById("channel-avatar")
    header_avatar.innerHTML = `<img src="${person.icon}" alt="${person.name}" width="40" />`

    let mainfeed = document.getElementById("mainfeed")
    mainfeed.innerHTML = ""

    for(let msg of person.messages) {
      mainfeed.innerHTML += createMessageHTML(msg.from, msg.text, msg.time)
    }

    //Add the selected tab class to the right one
    let dm_contacts = document.getElementById("dm_contacts")
    for(let c of dm_contacts.children) {
      c.classList.remove("direct-messages-selected")
    }

    let contactNode = document.getElementById(`dm_contact_${id}`)
    contactNode.classList.add("direct-messages-selected")
  }

  let dm_contacts = document.getElementById("dm_contacts")

  dm_contacts.innerHTML = ""
  for(let idx in People) {
    let person = People[idx]

    dm_contacts.innerHTML += `
    <a href="#" id="dm_contact_${idx}" onclick='switchToChat(${idx})'>
        <li>
            <i class="fas fa-circle online"></i>${person.displayName}
        </li>
    </a>
    `
  }

  switchToChat(0)

  let textBox = document.getElementById("textBox")
  textBox.addEventListener("keypress", (e) => {
    if (e.key === 'Enter' || e.keyCode === 13) {
        e.preventDefault();

        if(textBox.textContent != "") {
          sendMessage(textBox.textContent)
          textBox.textContent = ""
        }
    }
  })

})();