function waitTime(time) {
  return new Promise(resolve => {
    setTimeout(() => {
      resolve()
    }, time * 1000)
  })
}

Object.defineProperty(Array.prototype, "random", {
  value: function () {
    return this[Math.floor((Math.random()*this.length))];
  },
  enumerable: false
});

(async () => 
{
  //For web debugging
  if(typeof CefSharp === "undefined") {
    CefSharp = {}
    CefSharp.BindObjectAsync = () => {}

    game = {}
    game.readFile = (path) => {
      let str = ""
      if(path == "Content/Web/Chat/people/Delores/responses.json") {
        str = `[
          "Hello!",
          "Hi!",
          "Woah!",
          "Nope!",
          "Maybe!",
          {
              "filter": "\\\\?$",
              "msg": "I don't have time for your questions."
          },
          {
              "condition": "return msg.length > 15",
              "sequence": [
                  {
                      "typing": false,
                      "wait": 0.1
                  },
                  {
                      "typing": true,
                      "wait": 1.5
                  },
                  {
                      "typing": false,
                      "wait": 0.3
                  },
                  {
                      "typing": true,
                      "wait": 0.5
                  },
                  {
                      "msg": "I don't really even know how to respond. Figure it out yourself."
                  }
              ]
          }
      ]`
      }
      else {
        str = `[
          "Welcome to Forward Thinking Energy - we’re happy to have you here! Your role is critical to our success so please pay attention to the following instructions.",
          "In front of you is a button. If you press this button by clicking it with your mouse, you will cool down the reactor. You must press this button to prevent the reactor from overheating. <b>DO NOT allow the reactor to reach the critical heat level!</b> If this happens, the reactor will enter meltdown protocol and this scenario is grounds for immediate employment termination.",
          "This should be simple enough but feel free to reach out to one of your co-workers if you have any questions. You can do this by clicking on the name of one of your co-workers in the chat window. Any meetings or messages from your co-workers will appear here as well.",
          "I think that’s all for now! Keep an eye on those heat levels and don’t forget to press that button."
      ]`
      }

      return new Promise( (resolve) => { resolve(str)});
    }
  }

  await CefSharp.BindObjectAsync("game", "bound");

  let lockSwitching = false
  let People = []

  function addPerson(name, icon) {
    let person = {
      name: name,
      displayName: name,
      icon: `people/${name}/icon.png`,
      responses: [],
      id: People.length,
      inCustomChat: false,
      isTyping: false,
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

  function setTyping(person, isTyping) {
    person.isTyping = isTyping
    if(person == SelectedPerson) {
      document.getElementById("person_is_typing").style.visibility = isTyping ? "visible" : "hidden"
    }
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

  let you = addPerson("Player");
  you.displayName = "Player (You)"
  you.icon = "people/Player/icon.png"

  addPerson("Administrator")
  addPerson("Adrian")
  addPerson("Aida")
  addPerson("Christopher")
  addPerson("Delores")
  addPerson("Janey")
  addPerson("Jude")
  addPerson("Kailee")
  //addPerson("Mystery Person")
  addPerson("Quinn")


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


  function createMessageHTML(fromPerson, message) {

    return `
    <article class="feed">
      <section class="feeds-user-avatar">
        <img src="${fromPerson.icon}" alt="${fromPerson.name}" width="40" />
      </section>
      <section class="feed-content">
        <section class="feed-user-info">
          <h4>${fromPerson.name} </h4>
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

  function recieveMessage(person, text) {
    let response_msg = addMessage(person, person, text, "now")

    if(person == SelectedPerson) {
      let mainfeed = document.getElementById("mainfeed")
      let toPerson = SelectedPerson
      mainfeed.innerHTML += createMessageHTML(response_msg.from, response_msg.text)

      mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
    }
    else {
      let contactNode = document.getElementById(`dm_contact_${person.id}`).children[0]
      let notificationNode = contactNode.querySelector(".mention-badge")
      if(notificationNode == null) {
        notificationNode = document.createElement("span")
        notificationNode.classList.add("mention-badge")
        notificationNode.textContent = "0"
        contactNode.appendChild(notificationNode)
      }

      notificationNode.textContent = parseInt(notificationNode.textContent) + 1
    }
  }

  async function sendAsyncResponse(person, msg) {
    setTyping(person, true)
    await waitTime(msg.length * 0.01)
    setTyping(person, false)
    recieveMessage(person, msg)
  }

  async function handleResponseObject(person, response) {
    if(response.typing !== undefined) {
      setTyping(person, response.typing)
    }

    if(response.wait !== undefined) {
      await waitTime(response.wait)
    }

    if(response.msg !== undefined) {
      await sendAsyncResponse(person, response.msg)
    }

    if(response.sequence !== undefined) {
      for(let s of response.sequence) {
        await (handleResponseObject(person, s))
      }
    }
  }

  function generateResponseObjects(list) {
    let responseList = []
    for(let r of list) {
      if(typeof r === "string") {
        responseList.push({ msg: r})
      }
      else {
        responseList.push(r)
      }
    }

    return responseList
  }

  function handleResponse(person, message) {
    if(person.responses.length == 0) {
      return
    }

    //@TODO: Maybe do some custom handling for talking while in a sequence
    if(person.inCustomChat) {
      return
    }

    let responseList = generateResponseObjects(person.responses)

    //First, check all our filters
    let matchedFilters = []
    for(let r of responseList) {
      if(r.filter !== undefined) {
        let filter = new RegExp(r.filter)
        if(filter.test(message) == true) {
          matchedFilters.push(r)
        }
      }

      if(r.condition !== undefined) {
        let conditionFn = new Function('msg', r.condition)
        let conditionRet = conditionFn(message)
        if(conditionRet === true) {
          matchedFilters.push(r)
        }
      }
    }

    if(matchedFilters.length > 0) {
      setTimeout(async () => { await handleResponseObject(person, matchedFilters.random()) }, 0);
      return
    }

    //Remove all filters
    responseList = responseList.filter((v) => v.filter === undefined && v.condition === undefined)
    setTimeout(async () => { await handleResponseObject(person, responseList.random()) }, 0);
  
  }

  let DOING_SLASH_COMMAND = false

  function sendMessage(text) {
    let mainfeed = document.getElementById("mainfeed")

    let toPerson = SelectedPerson
    let msg = addMessage(toPerson, you, text, "now")
    mainfeed.innerHTML += createMessageHTML(msg.from, msg.text)

    handleResponse(toPerson, text)

    if(toPerson == you) {
      if(text.startsWith("/test")) {
        let regex = /\/test\s+(\w+)\s+(\w+)/
        let matches = text.match(regex)
        if(matches != null && matches[1] !== undefined && matches[2] !== undefined) {
          let p = getPerson(matches[1])
          if(p === undefined) {
            recieveMessage(you, `No person with the name ${matches[1]}`)
          }
          
          DOING_SLASH_COMMAND = true
          runCustomChat(matches[1], matches[2])
          DOING_SLASH_COMMAND = false
        }
      }
      else if(text.startsWith("/clearMessages")) {
        for(let p of People) {
          p.messages = []
        }
        mainfeed.innerHTML = ""
      }
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
      mainfeed.innerHTML += createMessageHTML(msg.from, msg.text)
    }
    mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;

    //Add the selected tab class to the right one
    let dm_contacts = document.getElementById("dm_contacts")
    for(let c of dm_contacts.children) {
      c.classList.remove("direct-messages-selected")
    }

    let contactNode = document.getElementById(`dm_contact_${person.id}`)
    contactNode.classList.add("direct-messages-selected")

    let notificationNode = contactNode.children[0].querySelector(".mention-badge")
    if(notificationNode != null) {
      contactNode.children[0].removeChild(notificationNode)
    }

    let textBox = document.getElementById(`textBox`)
    textBox.setAttribute("data-placeholder", `Message ${person.name}`)
    textBox.textContent = ""

    let person_is_typing = document.getElementById("person_is_typing")
    person_is_typing.textContent = `${person.name} is typing`
    setTyping(SelectedPerson, SelectedPerson.isTyping) //Silly, but avoids duplicate code for setting visibility
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
    <a style="cursor: pointer" id="dm_contact_${idx}" onclick='switchToChat(${idx})'>
        <li>
            <i class="fas fa-circle online"></i> <span class="displayName">${person.displayName}</span>
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
    //switchToPerson(person)
    //lockSwitching = true
    let doing_slash = DOING_SLASH_COMMAND
    game.readFile(`Content/Web/Chat/people/${person.name}/${name}.json`).then(async (r) => {
      if(r != "") {
        if(doing_slash) {
          recieveMessage(you, `Running Content/Web/Chat/people/${person.name}/${name}.json`)
        }

        let customChat = JSON.parse(r)
        person.inCustomChat = true

        let person_is_typing = document.getElementById("person_is_typing")
        let responses = generateResponseObjects(customChat)
        for(let r of responses) {
          await handleResponseObject(person, r)
          if(r.msg !== undefined) {
            await waitTime(r.msg.length * 0.024)
          }
        }

        //lockSwitching = false
        person.inCustomChat = false
      }
    })
  }

  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 0)
  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 1000)
  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 2000)
  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 3000)

  //runCustomChat("Delores", "intro_chat")
  //runCustomChat("Christopher", "security_check")

})();