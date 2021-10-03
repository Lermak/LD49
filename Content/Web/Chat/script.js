function waitTime(time) {
  return new Promise(resolve => {
    setTimeout(() => {
      resolve()
    }, time * 1000)
  })
}

(async () => 
{
  //For web debugging
  if(typeof CefSharp === "undefined") {
    CefSharp = {}
    CefSharp.BindObjectAsync = () => {}

    game = {}
    game.readFile = (path) => {
      return new Promise( (resolve, reject) => { resolve(`[
        "Welcome to Forward Thinking Energy - we’re happy to have you here! Your role is critical to our success so please pay attention to the following instructions.",
        "In front of you is a button. If you press this button by clicking it with your mouse, you will cool down the reactor. You must press this button to prevent the reactor from overheating. <b>DO NOT allow the reactor to reach the critical heat level!</b> If this happens, the reactor will enter meltdown protocol and this scenario is grounds for immediate employment termination.",
        "This should be simple enough but feel free to reach out to one of your co-workers if you have any questions. You can do this by clicking on the name of one of your co-workers in the chat window. Any meetings or messages from your co-workers will appear here as well.",
        "I think that’s all for now! Keep an eye on those heat levels and don’t forget to press that button."
    ]`)});
    }
  }

  await CefSharp.BindObjectAsync("game", "bound");

  let lockSwitching = false
  let People = []

  function addPerson(name, icon) {
    let person = {
      name: name,
      displayName: name,
      icon: "images/" + icon + ".png",
      responses: [],
      id: People.length,
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

  function getPerson(person) {
    if(typeof person === "string") {
      person = People.find((p) => {return p.name == person} )
    }

    return person
  }

  function addMessage(personMessage, fromPerson, message, time) {
    personMessage = getPerson(personMessage)
    fromPerson = getPerson(fromPerson)

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

  function recieveMessage(text) {
    let mainfeed = document.getElementById("mainfeed")
    let toPerson = SelectedPerson
    let response_msg = addMessage(toPerson, toPerson, text, "now")
    mainfeed.innerHTML += createMessageHTML(response_msg.from, response_msg.text, response_msg.time)

    mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
  }

  function sendMessage(text) {
    let mainfeed = document.getElementById("mainfeed")

    let toPerson = SelectedPerson
    let msg = addMessage(toPerson, you, text, "now")
    mainfeed.innerHTML += createMessageHTML(msg.from, msg.text, msg.time)

    if(toPerson.responses.length > 0) {
      recieveMessage(toPerson.responses[0])
    }

    mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
  }

  function switchToPerson(person) {
    if(lockSwitching) return;
    SelectedPerson = person

    let header = document.getElementById("channel-name")
    header.innerHTML = person.displayName

    let header_avatar = document.getElementById("channel-avatar")
    header_avatar.innerHTML = `<img src="${person.icon}" alt="${person.name}" width="40" />`

    let mainfeed = document.getElementById("mainfeed")
    mainfeed.innerHTML = `<div style="flex: 1"></div>`

    for(let msg of person.messages) {
      mainfeed.innerHTML += createMessageHTML(msg.from, msg.text, msg.time)
    }

    //Add the selected tab class to the right one
    let dm_contacts = document.getElementById("dm_contacts")
    for(let c of dm_contacts.children) {
      c.classList.remove("direct-messages-selected")
    }

    let contactNode = document.getElementById(`dm_contact_${person.id}`)
    contactNode.classList.add("direct-messages-selected")

    let textBox = document.getElementById(`textBox`)
    textBox.setAttribute("data-placeholder", `Message ${person.name}`)
    textBox.textContent = ""

    let person_is_typing = document.getElementById("person_is_typing")
    person_is_typing.textContent = `${person.name} is typing`
    person_is_typing.style.visibility = "hidden"
  }

  window.switchToChat = (id) => {
    let person = People[id]
    switchToPerson(person)
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

  window.sendChatMessage = () => {
    let textBox = document.getElementById("textBox")
    if(textBox.textContent != "") {
      sendMessage(textBox.textContent)
      textBox.textContent = ""
    }
  }

  let textBox = document.getElementById("textBox")
  textBox.addEventListener("keypress", (e) => {
    if (e.key === 'Enter' || e.keyCode === 13) {
        e.preventDefault();
        sendChatMessage();
    }
  })



  function runCustomChat(person, name) {
    person = getPerson(person)
    switchToPerson(person)
    lockSwitching = true

    game.readFile(`Content/Web/Chat/people/${person.name}/${name}.json`).then(async (r) => {
      if(r != "") {
        let customChat = JSON.parse(r)
        let person_is_typing = document.getElementById("person_is_typing")
        for(let msg of customChat) {
          if(typeof msg === "string") {
            person_is_typing.style.visibility = "visible";
            await waitTime(msg.length * 0.006)
            person_is_typing.style.visibility = "hidden";
            recieveMessage(msg)
            await waitTime(msg.length * 0.024)
          }

          lockSwitching = false
        }
      }
    })
  }

  runCustomChat("Delores", "intro_chat")

})();