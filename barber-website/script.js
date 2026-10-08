// Mobile nav toggle
const toggle = document.querySelector('.nav-toggle');
const nav = document.getElementById('nav');
toggle.addEventListener('click', () => {
  const open = nav.classList.toggle('open');
  toggle.setAttribute('aria-expanded', open);
});
nav.querySelectorAll('a').forEach(a =>
  a.addEventListener('click', () => {
    nav.classList.remove('open');
    toggle.setAttribute('aria-expanded', false);
  })
);

// Footer year
document.getElementById('year').textContent = new Date().getFullYear();

// Booking form: block past dates and Sundays, validate, show confirmation.
// No backend yet, so this only shows a confirmation message.
const form = document.getElementById('booking-form');
const msg = document.getElementById('form-msg');
form.date.min = new Date().toISOString().split('T')[0];

form.addEventListener('submit', e => {
  e.preventDefault();
  msg.className = 'form-msg';

  if (!form.checkValidity()) {
    msg.textContent = 'Please fill in all required fields.';
    msg.classList.add('err');
    return;
  }
  // Parse as local date to avoid timezone shifting the weekday
  const [y, m, d] = form.date.value.split('-').map(Number);
  if (new Date(y, m - 1, d).getDay() === 0) {
    msg.textContent = 'Sorry, we are closed on Sundays. Please pick another day.';
    msg.classList.add('err');
    return;
  }
  msg.textContent = `Thanks ${form.elements["name"].value}! We'll confirm your ${form.service.value} on ${form.date.value} at ${form.time.value} shortly.`;
  msg.classList.add('ok');
  form.reset();
});
