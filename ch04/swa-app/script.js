async function getUserInfo() {
    const response = await fetch('/.auth/me');
    const payload = await response.json();
    const { clientPrincipal } = payload;
    return clientPrincipal;
  }
  
  (async () => {
    const user = await getUserInfo();
    
    if (user) {
      document.getElementById('user-id').innerText = user.userId;
      document.getElementById('user-name').innerText = user.claims[0].name;
      document.getElementById('user-email').innerText = user.userDetails;
      document.getElementById('user-roles').innerText = user.userRoles;
      document.getElementById('user-info-block').style.display = 'block';
    } else {
      document.getElementById('user-info-block').style.display = 'none';
    }
  })();