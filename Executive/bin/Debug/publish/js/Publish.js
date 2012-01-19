/*
 * - Publish.js - Javascript that implements client operations on published code files
 * - ver 1.0 : 21 Aug 2011
 * - Software Modeling and Analysis, Project #2 result prototype, Fall 2011
 * - Jim Fawcett, Syracuse University
 */

/* footer is an HTML5 element.  The following statement supports fallback */
/* for older browsers that don't recognize this element.                  */

document.createElement("footer");

/* All of the Javascript below constitutes a single anonymous function, */
/* declared on the first line. It contains a number of other anonymous  */
/* functions that implement all client operations.                      */
/* Note that all of the code below uses the jQuery Javascript library.  */

$(document).ready(function () {

  /* toggle visibility of element after elements with CSS class "namespace" */

  $(".namespace").click(function () {
    if ($(this).next().is(":visible")) {
      $(this).next().hide();
    }
    else {
      $(this).next().show();
    }
  })
  /* toggle visibility of element after elements with CSS class "class" */

  $(".class").click(function () {
    if ($(this).next().is(":visible")) {
      $(this).next().hide();
    }
    else {
      $(this).next().show();
    }
  })
  /* toggle visibility of element after elements with CSS class "function" */

  $(".function").click(function () {
    if ($(this).next().is(":visible")) {
      $(this).next().hide();
    }
    else {
      $(this).next().show();
    }
  })
  /* toggle visibility of all elements after elements with CSS class "class" */
  /* when element with id "classbutton" is clicked                           */

  $("#classbutton").click(function () {
    if ($(".class").next().is(":visible")) {
      $(".class").next().hide();
    }
    else {
      $(".class").next().show();
    }
  })
  /* toggle visibility of all elements after elements with CSS class "function" */
  /* when element with id "functionbutton" is clicked                           */

  $("#functionbutton").click(function () {
    if ($(".function").next().is(":visible")) {
      $(".function").next().hide();
    }
    else {
      $(".function").next().show();
    }
  })
  /* toggle visibility of all elements with CSS class "net" */
  /* when element with id "netbutton" is clicked            */

  $("#netbutton").click(function () {
    if ($(".net").is(":visible")) {
      $(".net").hide();
    }
    else {
      $(".net").show();
    }
  })
  /* toggle visibility of all elements with CSS class "manpage" */
  /* when element with id "manpagebutton" is clicked            */

  $("#manpagebutton").click(function () {
    if ($(".manpage").is(":visible")) {
      $(".manpage").hide();
    }
    else {
      $(".manpage").show();
    }
  })
  /* toggle visibility of button elements */

  $("#buttonbutton").click(function () {
    if ($("#manpagebutton").is(":visible")) {
      $("#manpagebutton").hide();
    }
    else {
      $("#manpagebutton").show();
    }
    if ($("#classbutton").is(":visible")) {
      $("#classbutton").hide();
    }
    else {
      $("#classbutton").show();
    }
    if ($("#functionbutton").is(":visible")) {
      $("#functionbutton").hide();
    }
    else {
      $("#functionbutton").show();
    }
    if ($("#netbutton").is(":visible")) {
      $("#netbutton").hide();
    }
    else {
      $("#netbutton").show();
    }
  })
});
/* when page is loaded hide all the elements with CSS classes */
/* "function", "class", "manpage", and "net"                  */

$(window).load(function () {
  $(".function").next().hide();
  $(".class").next().hide();
  $(".manpage").hide();
  $(".net").hide();
});
