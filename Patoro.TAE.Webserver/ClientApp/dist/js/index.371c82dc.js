(function(t){function e(e){for(var r,u,i=e[0],s=e[1],a=e[2],d=0,l=[];d<i.length;d++)u=i[d],Object.prototype.hasOwnProperty.call(o,u)&&o[u]&&l.push(o[u][0]),o[u]=0;for(r in s)Object.prototype.hasOwnProperty.call(s,r)&&(t[r]=s[r]);p&&p(e);while(l.length)l.shift()();return c.push.apply(c,a||[]),n()}function n(){for(var t,e=0;e<c.length;e++){for(var n=c[e],r=!0,i=1;i<n.length;i++){var s=n[i];0!==o[s]&&(r=!1)}r&&(c.splice(e--,1),t=u(u.s=n[0]))}return t}var r={},o={index:0},c=[];function u(e){if(r[e])return r[e].exports;var n=r[e]={i:e,l:!1,exports:{}};return t[e].call(n.exports,n,n.exports,u),n.l=!0,n.exports}u.m=t,u.c=r,u.d=function(t,e,n){u.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},u.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},u.t=function(t,e){if(1&e&&(t=u(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(u.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var r in t)u.d(n,r,function(e){return t[e]}.bind(null,r));return n},u.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return u.d(e,"a",e),e},u.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},u.p="/";var i=window["webpackJsonp"]=window["webpackJsonp"]||[],s=i.push.bind(i);i.push=e,i=i.slice();for(var a=0;a<i.length;a++)e(i[a]);var p=s;c.push([0,"chunk-vendors"]),n()})({0:function(t,e,n){t.exports=n("56d7")},"2b54":function(t,e,n){"use strict";n("5270")},5270:function(t,e,n){},"56d7":function(t,e,n){"use strict";n.r(e);n("e260"),n("e6cf"),n("cca6"),n("a79d"),n("f9e3");var r=n("7a23"),o={class:"input-line"},c={class:"prompt"};function u(t,e,n,u,i,s){return Object(r["d"])(),Object(r["b"])("form",{class:"console",onSubmit:e[4]||(e[4]=Object(r["i"])((function(t){return s.sendCmd()}),["prevent"]))},[Object(r["c"])("div",{ref:"output",class:"output",innerHTML:s.output},null,8,["innerHTML"]),Object(r["c"])("div",o,[Object(r["c"])("div",c,Object(r["e"])(i.prompt),1),Object(r["g"])(Object(r["c"])("input",{ref:"input",type:"text","onUpdate:modelValue":e[1]||(e[1]=function(t){return i.input=t}),onKeyup:[e[2]||(e[2]=Object(r["h"])(Object(r["i"])((function(t){return s.lastEntry(!0)}),["stop"]),["up"])),e[3]||(e[3]=Object(r["h"])(Object(r["i"])((function(t){return s.lastEntry(!1)}),["stop"]),["down"]))]},null,544),[[r["f"],i.input]])])],32)}n("a15b"),n("d3b7"),n("25f0"),n("96cf");var i=n("1da1"),s=n("e87a"),a=(new s["a"]).withUrl("http://localhost:5000/game").withAutomaticReconnect().build(),p={name:"App",data:function(){return{messages:[],prompt:"$",input:"",commands:[],commandIdx:-1}},mounted:function(){var t=this;this.scrollToBottom(),this.$refs.input.focus(),a.on("msg",(function(e){t.addMessage(e)}))},computed:{output:function(){return this.messages.join("<br>")}},methods:{scrollToBottom:function(){this.$refs.output.scrollTop=this.$refs.output.scrollHeight},addMessage:function(t){var e=this;this.messages.push(t),this.$nextTick((function(){return e.scrollToBottom()}))},sendCmd:function(){var t=this;return Object(i["a"])(regeneratorRuntime.mark((function e(){var n;return regeneratorRuntime.wrap((function(e){while(1)switch(e.prev=e.next){case 0:if(n=t.input,t.commands.push(n),t.commandIdx=t.commands.length,e.prev=3,"Connected"===a.state){e.next=7;break}return e.next=7,a.start();case 7:return t.addMessage("You: ".concat(n)),e.next=10,a.invoke("cmd",n);case 10:t.input="",e.next=16;break;case 13:e.prev=13,e.t0=e["catch"](3),console.error(e.t0.toString());case 16:case"end":return e.stop()}}),e,null,[[3,13]])})))()},lastEntry:function(t){if(console.log(t?"UP":"DOWN",this.commands,this.commandIdx),this.commands.length){t?this.commandIdx--:this.commandIdx++,this.commandIdx=Math.max(0,Math.min(this.commands.length-1,this.commandIdx));var e=this.commands[this.commandIdx];this.input=e}}}};n("2b54");p.render=u;var d=p;Object(r["a"])(d).mount("#app")}});
//# sourceMappingURL=index.371c82dc.js.map