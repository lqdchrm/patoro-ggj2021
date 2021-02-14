<template>
    <form class="console" @submit.prevent="sendCmd()">
      <div ref="output" class="output" v-html="output"></div>
      <div class="input-line">
        <div class="prompt">{{prompt}}</div>
        <input ref="input" type="text" v-model="input">
      </div>
    </form>
</template>

<script>
import * as signalr from "@microsoft/signalr";

const connection = new signalr.HubConnectionBuilder()
  .withUrl("http://localhost:5000/game")
  .withAutomaticReconnect()
  .build();

export default {
  name: 'App',
  data() {
    return {
      messages: [],
      prompt: "$",
      input: ""
    }
  },
  mounted() {
    this.scrollToBottom();
    this.$refs.input.focus();
    connection.on("msg", (message) => {
      console.log("Received: ", message);
      this.addMessage(message);
    });
  },
  computed: {
    output() {
      return this.messages.join("<br>")
    }
  },
  methods: {
    scrollToBottom() {
      this.$refs.output.scrollTop = this.$refs.output.scrollHeight;
    },
    addMessage(msg) {
      this.messages.push(msg);
      this.$nextTick(() => this.scrollToBottom());
    },
    async sendCmd() {
      let cmd = this.input
      try {
        if (connection.state !== "Connected") await connection.start();

        this.addMessage(`You: ${cmd}`);
        await connection.invoke("cmd", cmd);
        this.input = "";
      } catch(err) {
        console.error(err.toString());
      }
    }
  }
}
</script>

<style>
@import url('http://fonts.cdnfonts.com/css/cousine');

* {
  margin: 0;
  padding: 0;
  background: transparent;
  color: #080;
}

html, body {
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  margin: 0;
  padding: 10px;
  width: 100%;
  height: 100%;
  background: black;
}

::-webkit-scrollbar {
  width: 10px;
}

::-webkit-scrollbar-track {
  background: #020;
}

::-webkit-scrollbar-thumb {
  background: #040;
}

::-webkit-scrollbar-thumb:hover {
  background: #060;
}

#app {
  font-family: cousine, monospace;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  height: 100%;
}

.console {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: stretch;
  overflow: hidden;
  padding: 0 5%;
}

.console > * {
  flex: 0 1 auto;
}

.console table {
  line-height: 1.2em;
}

.console > .output {
  background: #020;
  flex: 1 1 auto;
  overflow-y: scroll;
  padding: 4px;
  border-style: hidden;
  outline: none;
  resize: none;
}

.console td {
  text-align: center;
}

.console .item {
  color: darkred;
}

.console > .input-line {
  margin-top: 10px;
  background: #020;
  color: #080;
  display: flex;
}

.console > .input-line > .prompt {
  flex: 0 1 auto;
  padding-right: 0.5em;
}

.console > .input-line > input {
  flex: 1 1 auto;
  border-style: hidden;
  outline: none;
  background: #020;
  color: #080;
}
</style>
