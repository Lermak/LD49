let SUPER_SPEED = 1
let JUDE_MODE = false
let SOULS = 165843
let global_data = {}

function waitTime(time) {
  return new Promise(resolve => {
    setTimeout(() => {
      resolve()
    }, time * 1000 * (1 / SUPER_SPEED))
  })
}

/*
some way of running code in response
erorrs as chat messages from player
File loading working in publish
*/

function getRandomInt(min, max) {
  min = Math.ceil(min);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min) + min); //The maximum is exclusive and the minimum is inclusive
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
    game.notify = () => {}
    game.ready = () => {}
    game.playSound = () => {}
    game.sendEvent = () => {}
    game.readFile = (path) => {
      return new Promise((resolve) => {
        var rawFile = new XMLHttpRequest();
        rawFile.open("GET", "http://localhost:8080/" + path, true);
        rawFile.addEventListener("load", () => { resolve(rawFile.responseText); });
        rawFile.send();
      });

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
        str = `[{
          "uniqueId": 1,
          "msg": "I will only say this once."
      },
      {
          "uniqueId": 1,
          "msg": "I will also only say this once, shared with above."
      }, "Other"]`
      }

      return new Promise( (resolve) => { resolve(str)});
    }
  }

  await CefSharp.BindObjectAsync("game", "bound");

  let lockSwitching = false
  let People = []

  function addPerson(name, icon) {
    let person = {
      idName: name,
      name: name,
      displayName: name,
      icon: `people/${name}/icon.png`,
      responses: [],
      id: People.length,
      inCustomChat: false,
      queuedChats: [],
      isTyping: false,
      uniqueIds: {},
      messages: [],
      unread: 0,
      messagesToJude: []
    }

    person.loadResponses = () => {
      game.readFile(`Content/Web/Chat/people/${person.idName}/responses.json`).then((r) => {
        if(r != "") {
          person.responses = JSON.parse(r)
        }
      })
    }

    person.loadResponses();
    People.push(person)
    return person
  }

  let currentCthulhuName = ""
  function generateNameHTML(person, extraClass, time) {
    if(time === undefined) time = ""

    let name = person.name
    if(person.cthulhu !== undefined) name = currentCthulhuName
    
    return `<div class="${extraClass}"> <span ${person.cthulhu !== undefined ? `class="cthulhu" data-text="${currentCthulhuName}"` : ""}">${name}</span> <span class="time-stamp">${time}</span> </div>`
  }

  function setTyping(person, isTyping) {
    person.isTyping = isTyping

    if(JUDE_MODE) {
      document.getElementById("person_is_typing").style.visibility = "hidden"
      return
    }

    if(person == SelectedPerson) {
      document.getElementById("person_is_typing").style.visibility = isTyping ? "visible" : "hidden"
    }
  }

  function getPerson(person) {
    if(typeof person === "string") {
      person = People.find((p) => {return p.idName == person} )
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

  addPerson("Administrator")
  addPerson("Adrian")
  let tim = addPerson("Tim")
  tim.name = "Tim@IT"
  tim.displayName = "Tim@IT"
  
  addPerson("Aida")
  addPerson("Christopher")
  addPerson("Delores")
  addPerson("Janey")
  
  let jude = addPerson("Jude")
  let you = addPerson("Player")
  you.name = "New Hire"
  you.displayName = "New Hire (You)"
  you.icon = "people/Player/icon.png"

  addPerson("Kailee")
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


  function createMessageHTML(fromPerson, message, time) {
    return `
    <article class="feed">
      <section class="feeds-user-avatar">
        <img src="${fromPerson.icon}" alt="${fromPerson.name}" width="40" />
      </section>
      <section class="feed-content">
        <section class="feed-user-info">
          <h4>${generateNameHTML(fromPerson, "", time)}</h4>
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

  let delayed_messages = []
  let process_delay = false
  function recieveMessage(person, text) {
    if(JUDE_MODE) {
      delayed_messages.push([person, text])
      game.playSound("MessageNotification")
      return
    }

    let response_msg = addMessage(person, person, text)

    if(person == jude) {
      you.messagesToJude.push({
        from: person,
        text: text
      })
    }

    if(person == SelectedPerson) {
      let mainfeed = document.getElementById("mainfeed")
      let toPerson = SelectedPerson
      mainfeed.innerHTML += createMessageHTML(response_msg.from, response_msg.text)

      game.playSound("MessagePop")
      mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
    }
    else {
      person.unread += 1

      let contactNode = document.getElementById(`dm_contact_${person.id}`).children[0]
      let notificationNode = contactNode.querySelector(".mention-badge")
      if(notificationNode == null) {
        notificationNode = document.createElement("span")
        notificationNode.classList.add("mention-badge")
        contactNode.appendChild(notificationNode)
      }

      if(!process_delay) game.playSound("MessageNotification")
      notificationNode.textContent = person.unread
    }
  }

  async function sendAsyncResponse(person, msg) {
    setTyping(person, true)
    await waitTime(0.25 + msg.length * 0.01)
    setTyping(person, false)
    recieveMessage(person, msg)
  }

  async function handleResponseObject(person, response) {
    if(response.uniqueId !== undefined) {
      if(person.uniqueIds[response.uniqueId] !== undefined) {
        return
      }

      person.uniqueIds[response.uniqueId] = true
    }

    if(response.requiresId !== undefined) {
      if(person.uniqueIds[response.requiresId] !== undefined) {
        return
      }
    }

    if(response.typing !== undefined) {
      setTyping(person, response.typing)
    }

    if(response.wait !== undefined) {
      await waitTime(response.wait)
    }

    if(response.msg !== undefined) {
      await sendAsyncResponse(person, response.msg)
    }

    if(response.waitAfter === undefined) {
      if(response.msg !== undefined) {
        response.waitAfter = response.msg.length * 0.024
      }
      else {
        response.waitAfter = 0
      }
    }

    if(response.event !== undefined) {
      game.sendEvent(response.event)
    }

    await waitTime(response.waitAfter)

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

  function run_handler(ev, person, regexArray) {
    if(ev == "aida_setsalary") {
      let amount = parseInt(regexArray[1])
      let response = ""
      if(amount < 0) {
        reponse = "Please enter a valid salary from $0 to $150,000."
      }
      else if(amount >= 0 && amount <= 150000) {
        reponse = "Salary set."
      }
      else {
        reponse = "Please enter a valid salary from $0 to $150,000."
      }

      setTimeout(async () => { await sendAsyncResponse(person, reponse) }, 0);
    }
    else if(ev == "aida_soulcounts") {
      setTimeout(async () => { await sendAsyncResponse(person, `The Great Old One has aquired ${SOULS} souls.`) }, 0);
    }
    else if(ev == "aida_removeoverlay") {
      game.sendEvent("remove_overlay")
      global_data.remove_overlay = true
      setTimeout(async () => { await sendAsyncResponse(person, `Overlay Removed.`) }, 0);
    }
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

    //Filter out unique responses that have already happened
    responseList = responseList.filter((v) => {
      if(v.requires) {
        let conditionFn = new Function('msg', 'global_data', v.requires)
        let conditionRet = conditionFn(message, global_data)
        return conditionRet
      }

      if(v.uniqueId !== undefined) {
        return person.uniqueIds[v.uniqueId] === undefined
      }

      if(v.requiresId !== undefined) {
        return person.uniqueIds[v.requiresId] === undefined
      }

      return true
    })

    //First, check all our filters
    let matchedFilters = []
    for(let r of responseList) {
      let forcedResponse = false

      if(r.filter !== undefined) {
        let filter = new RegExp(r.filter)
        if(filter.test(message.toLowerCase()) == true) {
          if(r.handler !== undefined) {
            run_handler(r.handler, person, message.toLowerCase().match(filter))
          }
          matchedFilters.push(r)
        }
      }

      if(r.condition !== undefined) {
        let conditionFn = new Function('msg', 'global_data', r.condition)
        let conditionRet = conditionFn(message, global_data)
        if(conditionRet === true) {
          matchedFilters.push(r)
        }
      }

      if(matchedFilters.includes(r) && r.force === true) {
        matchedFilters = []
        matchedFilters.push(r)
        break
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
    if(JUDE_MODE) return;

    let mainfeed = document.getElementById("mainfeed")

    let toPerson = SelectedPerson
    let msg = addMessage(toPerson, you, text)
    mainfeed.innerHTML += createMessageHTML(msg.from, msg.text)

    if(toPerson == jude) {
      you.messagesToJude.push({
        from: you,
        text: text
      })
    }

    game.playSound("MessagePopMe")

    if(toPerson == you && text.startsWith("/")) {
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
      else if(text.startsWith("/reload")) {
        for(let p of People) {
          p.uniqueIds = {}
          p.loadResponses()
        }
      }
    }
    else {
      handleResponse(toPerson, text)
    }

    mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
  }

  function switchToPerson(person) {
    if(lockSwitching) return;
    SelectedPerson = person

    let header = document.getElementById("channel-name")
    header.innerHTML =  `${generateNameHTML(person)}`

    let header_avatar = document.getElementById("channel-avatar")
    header_avatar.innerHTML = `<img src="${person.icon}" alt="${person.name}" width="40" />`

    let mainfeed = document.getElementById("mainfeed")
    mainfeed.innerHTML = `<div style="flex: 1"></div>`
    
    let messages = person.messages
    if(JUDE_MODE) {
      messages = person.messagesToJude
    }

    for(let msg of messages) {
      mainfeed.innerHTML += createMessageHTML(msg.from, msg.text, msg.time)
    }

    if(!JUDE_MODE) {
      mainfeed.scrollTop = mainfeed.scrollHeight - mainfeed.clientHeight;
    }
    else {
      mainfeed.scrollTop = 0;
    }

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
    person.unread = 0;

    let textBox = document.getElementById(`textBox`)
    textBox.setAttribute("data-placeholder", `Message ${person.name}`)
    textBox.textContent = ""

    let person_is_typing = document.getElementById("person_is_typing")
    person_is_typing.innerHTML = `${generateNameHTML(person)} <span style="margin-left:0.2em">is typing</span>`
    setTyping(SelectedPerson, SelectedPerson.isTyping) //Silly, but avoids duplicate code for setting visibility
  }

  window.switchToChat = (id) => {
    let person = People[id]
    switchToPerson(person)
  }

  
  function generateContacts() {
    let dm_contacts = document.getElementById("dm_contacts")
    dm_contacts.innerHTML = ""
    for(let idx in People) {
      let person = People[idx]

      if(JUDE_MODE) {
        if(person == jude) continue
        if(person.idName == "Stranger") continue
      }
      else if(!JUDE_MODE && person == you) {
        continue
      }

      dm_contacts.innerHTML += `
      <a style="cursor: pointer" id="dm_contact_${idx}" onclick='switchToChat(${idx})'>
          <li>
              <i class="fas fa-circle online"></i> ${generateNameHTML(person, "displayName")} ${ (person.unread != 0 && !JUDE_MODE) ? `<span class="mention-badge">${person.unread}</span>` : "" }
          </li>
      </a>
      `
    }
  }
  generateContacts()

  switchToChat(0)

  window.sendChatMessage = () => {
    if(JUDE_MODE) return;

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



  function runCustomChat(person, name, force) {
    if(force === undefined) { force = false }

    
    person = getPerson(person)
    if(person.inCustomChat) {
      person.queuedChats.push([name, force])
      return
    }

    
    if(force) {
      switchToPerson(person)
      lockSwitching = true
    }
    person.inCustomChat = true

    let doing_slash = DOING_SLASH_COMMAND
    return new Promise(resolve => { game.readFile(`Content/Web/Chat/people/${person.idName}/${name}.json`).then(async (r) => {
      if(r != "") {
        if(doing_slash) {
          recieveMessage(you, `Running Content/Web/Chat/people/${person.idName}/${name}.json`)
        }

        let customChat = JSON.parse(r)
        let responses = generateResponseObjects(customChat)
        for(let r of responses) {
          await handleResponseObject(person, r)
        }
      }

      if(force) {
        lockSwitching = false
      }
      person.inCustomChat = false

      game.sendEvent(`${person.idName}_${name}`)

      if(person.queuedChats.length > 0) {
        let nextChat = person.queuedChats.shift()
        runCustomChat(person, nextChat[0], nextChat[1])
      }

      resolve()
    }) }) 
  }

  var zalgo_up = [
    '\u030d', /*     ̍     */   '\u030e', /*     ̎     */   '\u0304', /*     ̄     */   '\u0305', /*     ̅     */
    '\u033f', /*     ̿     */   '\u0311', /*     ̑     */   '\u0306', /*     ̆     */   '\u0310', /*     ̐     */
    '\u0352', /*     ͒     */   '\u0357', /*     ͗     */   '\u0351', /*     ͑     */   '\u0307', /*     ̇     */
    '\u0308', /*     ̈     */   '\u030a', /*     ̊     */   '\u0342', /*     ͂     */   '\u0343', /*     ̓     */
    '\u0344', /*     ̈́     */    '\u034a', /*     ͊     */   '\u034b', /*     ͋     */   '\u034c', /*     ͌     */
    '\u0303', /*     ̃     */   '\u0302', /*     ̂     */   '\u030c', /*     ̌     */   '\u0350', /*     ͐     */
    '\u0300', /*     ̀     */   '\u0301', /*     ́     */   '\u030b', /*     ̋     */   '\u030f', /*     ̏     */
    '\u0312', /*     ̒     */   '\u0313', /*     ̓     */   '\u0314', /*     ̔     */   '\u033d', /*     ̽     */
    '\u0309', /*     ̉     */   '\u0363', /*     ͣ     */   '\u0364', /*     ͤ     */   '\u0365', /*     ͥ     */
    '\u0366', /*     ͦ     */   '\u0367', /*     ͧ     */   '\u0368', /*     ͨ     */   '\u0369', /*     ͩ     */
    '\u036a', /*     ͪ     */   '\u036b', /*     ͫ     */   '\u036c', /*     ͬ     */   '\u036d', /*     ͭ     */
    '\u036e', /*     ͮ     */   '\u036f', /*     ͯ     */   '\u033e', /*     ̾     */   '\u035b', /*     ͛     */
    '\u0346', /*     ͆     */   '\u031a' /*     ̚     */
  ];
  
  //those go DOWN
  var zalgo_down = [
    '\u0316', /*     ̖     */   '\u0317', /*     ̗     */   '\u0318', /*     ̘     */   '\u0319', /*     ̙     */
    '\u031c', /*     ̜     */   '\u031d', /*     ̝     */   '\u031e', /*     ̞     */   '\u031f', /*     ̟     */
    '\u0320', /*     ̠     */   '\u0324', /*     ̤     */   '\u0325', /*     ̥     */   '\u0326', /*     ̦     */
    '\u0329', /*     ̩     */   '\u032a', /*     ̪     */   '\u032b', /*     ̫     */   '\u032c', /*     ̬     */
    '\u032d', /*     ̭     */   '\u032e', /*     ̮     */   '\u032f', /*     ̯     */   '\u0330', /*     ̰     */
    '\u0331', /*     ̱     */   '\u0332', /*     ̲     */   '\u0333', /*     ̳     */   '\u0339', /*     ̹     */
    '\u033a', /*     ̺     */   '\u033b', /*     ̻     */   '\u033c', /*     ̼     */   '\u0345', /*     ͅ     */
    '\u0347', /*     ͇     */   '\u0348', /*     ͈     */   '\u0349', /*     ͉     */   '\u034d', /*     ͍     */
    '\u034e', /*     ͎     */   '\u0353', /*     ͓     */   '\u0354', /*     ͔     */   '\u0355', /*     ͕     */
    '\u0356', /*     ͖     */   '\u0359', /*     ͙     */   '\u035a', /*     ͚     */   '\u0323' /*     ̣     */
  ];
  
  //those always stay in the middle
  var zalgo_mid = [
    '\u0315', /*     ̕     */   '\u031b', /*     ̛     */   '\u0340', /*     ̀     */   '\u0341', /*     ́     */
    '\u0358', /*     ͘     */   '\u0321', /*     ̡     */   '\u0322', /*     ̢     */   '\u0327', /*     ̧     */
    '\u0328', /*     ̨     */   '\u0334', /*     ̴     */   '\u0335', /*     ̵     */   '\u0336', /*     ̶     */
    '\u034f', /*     ͏     */   '\u035c', /*     ͜     */   '\u035d', /*     ͝     */   '\u035e', /*     ͞     */
    '\u035f', /*     ͟     */   '\u0360', /*     ͠     */   '\u0362', /*     ͢     */   '\u0338', /*     ̸     */
    '\u0337', /*     ̷     */   '\u0361', /*     ͡     */   '\u0489' /*     ҉_     */
  ];

  function generateCthulhu() {
    let prefix = [
      "FIND",
      "UNCOVER",
      "SEEK",
      "UNEARTH"
    ]

    let wordbank = [
      "SECRET",
      "TRUTH",
      "UNCOVER",
      "CONSPIRACY"
    ]

    let nameStr = ""
    for(let c = 0; c < 10; ++c) {
      nameStr += prefix.random()
      nameStr += wordbank.random()
    }

    let str = ""
    let numChars = getRandomInt(8, 20)
    for(let c = 0; c < numChars; ++c) {

      let numZalgo = getRandomInt(5, 30)
      for(let z = 0; z < numZalgo; ++z) {
        str += zalgo_mid.random();
      }

      numZalgo = getRandomInt(1, 5)
      for(let z = 0; z < numZalgo; ++z) {
        str += zalgo_up.random();
      }

      //numZalgo = getRandomInt(1, 5)
      //for(let z = 0; z < numZalgo; ++z) {
      //  str += zalgo_down.random();
      //}

      str += nameStr[c];
    }

    currentCthulhuName = str

    let elements = document.querySelectorAll(".cthulhu")
    for (i = 0; i < elements.length; i++) {
      elements[i].textContent = str
      elements[i].setAttribute("data-text", str)

      elements[i].classList.remove("cthulhu")
      void elements[i].offsetWidth;
      elements[i].classList.add("cthulhu")

    }
  }

  setInterval(() => {
    if(Math.random() > 0.666) {
      generateCthulhu()
    }
  }, 666)

  setInterval(() => {
    if(Math.random() > 0.4) {
      SOULS += Math.floor(Math.random() * 1000)
    }
  }, 666)

  window.runCustomChat = (person, chat, force) => {
    runCustomChat(person, chat, force)
  }

  window.toggleJudeMode = () => {
    JUDE_MODE = !JUDE_MODE
    let button = document.getElementById("jude_button")
    if(JUDE_MODE) {
      button.classList.remove("jude_button")
      button.classList.add("jude_button2")
      document.documentElement.style.setProperty("--slack-background", "#221322")
      document.documentElement.style.setProperty("--slack-other-bckground", "#1b131b")
      document.documentElement.style.setProperty("--slack-background-selected", "#5b355c")
      document.getElementById("textBox").setAttribute("contenteditable", "false")

      if(SelectedPerson == jude || SelectedPerson.idName == "Stranger") {
        SelectedPerson = you
      }
    }
    else {
      button.classList.remove("jude_button2")
      button.classList.add("jude_button")
      document.documentElement.style.setProperty("--slack-background", "#350d36")
      document.documentElement.style.setProperty("--slack-other-bckground", "#3f0e40")
      document.documentElement.style.setProperty("--slack-background-selected", "#832187")
      document.getElementById("textBox").setAttribute("contenteditable", "true")

      if(SelectedPerson == you) {
        SelectedPerson = jude
      }

      process_delay = true
      for(let a of delayed_messages) {
        recieveMessage(a[0], a[1])
      }
      process_delay = false
      delayed_messages = []
    }

    generateContacts()

    document.body.classList.remove("generic_glitch_oneshot")
    void document.body.offsetWidth;
    document.body.classList.add("generic_glitch_oneshot")

    switchToChat(SelectedPerson.id)
  }

  window.setGlobalValue = (v, value) => {
    global_data[v] = value
  }

  window.recieveGameEvent = (ev) => {
    if(ev == "meet_stranger") {
        setTimeout(async () => {
        let stranger = addPerson("Stranger")
        stranger.cthulhu = true
        stranger.name = "?????"
        stranger.displayName = "?????"
        generateCthulhu()
        generateContacts()
        game.playSound("StrangerArrive")

        await waitTime(2)
        await runCustomChat("Stranger", "first_contact")
        
        document.getElementById("jude_button").style.visibility = "visible"
        global_data.met_stranger = true
        game.sendEvent("met_stranger")

      }, 0)
    }
  }

  game.readFile(`Content/Web/Chat/people/Jude/logs.json`).then((r) => {
    if(r != "") {
      let jude_logs = JSON.parse(r)
      for(let person in jude_logs) {
        let p = getPerson(person)
        let chat = jude_logs[person]
        for(let m of chat) {
          let text = ""
          let fromPerson = jude;
          if(m.to_msg !== undefined) {
            text = m.to_msg;
          }
          if(m.from_msg !== undefined) {
            text = m.from_msg;
            fromPerson = p
          }
          p.messagesToJude.push({from: fromPerson, text: text, time: m.time});
        }
      }
    }
  })

  game.ready()

  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 0)
  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 1000)
  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 2000)
  //setTimeout(() => {recieveMessage(getPerson("Delores"), "Yo!")}, 3000)

  //runCustomChat("Delores", "salary_chat", false)
  //runCustomChat("Christopher", "security_check", false)
  //runCustomChat("Christopher", "morse_code", false)
  //runCustomChat("Christopher", "coworker_bot", false)

  //setTimeout(() => {recieveGameEvent("meet_stranger")}, 0)
  //document.getElementById("jude_button").style.visibility = "visible"
  //global_data.remove_overlay = true

})();