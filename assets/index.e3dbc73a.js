var A=Object.defineProperty,q=Object.defineProperties;var z=Object.getOwnPropertyDescriptors;var f=Object.getOwnPropertySymbols;var b=Object.prototype.hasOwnProperty,k=Object.prototype.propertyIsEnumerable;var x=(t,e,r)=>e in t?A(t,e,{enumerable:!0,configurable:!0,writable:!0,value:r}):t[e]=r,p=(t,e)=>{for(var r in e||(e={}))b.call(e,r)&&x(t,r,e[r]);if(f)for(var r of f(e))k.call(e,r)&&x(t,r,e[r]);return t},L=(t,e)=>q(t,z(e));var C=(t,e)=>{var r={};for(var n in t)b.call(t,n)&&e.indexOf(n)<0&&(r[n]=t[n]);if(t!=null&&f)for(var n of f(t))e.indexOf(n)<0&&k.call(t,n)&&(r[n]=t[n]);return r};import{j as s,c as N,r as l,u as $,a as B,b as v,G as h,T as E,B as F,C as H,d as M,H as U,R as I,e as m,N as P,f as G,o as K,g as D,h as J,i as Q,S as V}from"./vendor.f476e36d.js";const W=function(){const e=document.createElement("link").relList;if(e&&e.supports&&e.supports("modulepreload"))return;for(const o of document.querySelectorAll('link[rel="modulepreload"]'))n(o);new MutationObserver(o=>{for(const a of o)if(a.type==="childList")for(const i of a.addedNodes)i.tagName==="LINK"&&i.rel==="modulepreload"&&n(i)}).observe(document,{childList:!0,subtree:!0});function r(o){const a={};return o.integrity&&(a.integrity=o.integrity),o.referrerpolicy&&(a.referrerPolicy=o.referrerpolicy),o.crossorigin==="use-credentials"?a.credentials="include":o.crossorigin==="anonymous"?a.credentials="omit":a.credentials="same-origin",a}function n(o){if(o.ep)return;o.ep=!0;const a=r(o);fetch(o.href,a)}};W();function X(){return s("div",{children:"Logged in!"})}class Y{constructor(e){this.baseUrl=e,console.log(`Initialized ApiClient for ${e}`)}async baseRequest(e,r={}){const n=g.getState().token,c=r,{headers:o}=c,a=C(c,["headers"]),i=await fetch(`${this.baseUrl}/${e}`,p({headers:p(L(p({},n&&{Authorization:`Bearer ${n}`}),{"Content-Type":"application/json"}),o)},a));if(i.status===401)throw n!==void 0&&(console.log("Unauthorized, logging out"),g.getState().logOut()),new w;if(i.ok)return await i.json();throw new R(i)}async logIn(e,r){const n=new URLSearchParams({email:e,password:r});try{return await this.baseRequest(`user/login?${n}`,{method:"POST"})}catch(o){throw o instanceof R&&o.response.status===400?new w:o}}}class R extends Error{constructor(e){super();this.response=e}}const Z=new Y("https://localhost:7061");class w extends Error{}const T="token",g=N(t=>({token:localStorage[T],isLoggedIn(){return this.token!==void 0},async logIn(e,r){try{const n=await Z.logIn(e,r);return t(o=>({token:n})),!0}catch(n){if(n instanceof w)return!1;throw n}},logOut(){t(e=>({token:void 0}))}}));g.subscribe(t=>{localStorage[T]=t.token});function _(t){const[e,r]=l.exports.useState(!1),[n,o]=l.exports.useState(void 0),[a,i]=l.exports.useState(void 0),c=l.exports.useRef(!1);return l.exports.useEffect(()=>()=>{c.current=!0},[]),{call:l.exports.useCallback(async(...S)=>{o(void 0),i(void 0),r(!0);try{const u=await t(...S);return c.current||(i(u),r(!1)),u}catch(u){c.current||(o(u),r(!1))}},[t,c]),result:a,loading:e,error:n}}function ee(){const t=$(),e=g(),[r,n]=l.exports.useState(""),[o,a]=l.exports.useState(""),[i,c]=l.exports.useState(void 0),{call:y,loading:S,error:u}=_(e.logIn),{enqueueSnackbar:O}=B();async function j(){const d=await y(r,o);d!==void 0&&(d?O("Login successful",{variant:"success"}):c("wrong credentials"))}return v(h,{container:!0,direction:"column",rowSpacing:4,alignItems:"center",justifyContent:"center",style:{minHeight:"100vh"},children:[s(h,{item:!0,children:s(E,{label:"Email",value:r,error:!!i,helperText:i,onChange:d=>n(d.target.value)})}),s(h,{item:!0,children:s(E,{label:"Password",value:o,type:"password",error:!!i,helperText:i,onChange:d=>a(d.target.value)})}),s(h,{item:!0,children:s(F,{variant:"contained",size:"large",onClick:j,children:"Log in"})}),S&&s(h,{item:!0,children:s(H,{})}),u&&s(h,{item:!0,children:s(M,{color:t.palette.error.main,children:u.toString()})})]})}function te(){const t=g(e=>e.isLoggedIn());return s(U,{children:t?v(I,{children:[s(m,{path:"/",element:s(X,{})}),s(m,{path:"*",element:s(P,{to:"/",replace:!0})})]}):v(I,{children:[s(m,{path:"/login",element:s(ee,{})}),s(m,{path:"*",element:s(P,{to:"/login",replace:!0})})]})})}const re=G({palette:{primary:{main:K[500]}},components:{MuiTextField:{defaultProps:{variant:"outlined"}}}});D.render(s(J.StrictMode,{children:s(Q,{theme:re,children:s(V,{maxSnack:3,children:s(te,{})})})}),document.getElementById("root"));
