// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var options = document.getElementsByClassName('nav__menu__option');

for (let index = 0; index < options.length; index++) {
    const el = options[index];
    if(!el.hasChildNodes()) continue;
    
    for (let y = 0; y < el.children.length; y++) {
        const ch = el.children[y];
        
        if(ch.classList.contains('nav__menu__option__submenu')) {
            createSubmenu(el);
            console.log('Submenu created!')
            break;

        }
    }
}


function createSubmenu(el) {
    el.children[0].addEventListener('click', function submenuListener(e) {
        for (let x = 0; x < options.length; x++) {
            const option = options[x];
            
            if (option == el) {
                if (option.classList.contains('active')) {
                    option.classList.remove('active');
                    try{
                        document.removeEventListener('click', outsideOfBoxClick);
                    } catch {}
                    return;
                } else {
                    option.classList.add('active');
                    var firstTime = true;
                    document.addEventListener('click', function outsideOfBoxClick(e) {
                        const box = el.children;

                            if(box[0].contains(e.target)) {
                                if(firstTime) {
                                    firstTime = !firstTime;
                                } else {
                                    option.classList.remove('active');
                                    document.removeEventListener('click', outsideOfBoxClick);
                                }
                            } else if(!box[1].contains(e.target)) {
                                option.classList.remove('active');
                                document.removeEventListener('click', outsideOfBoxClick);
                            }
                    });
                }
            } else {
                if (option.classList.contains('active')) option.classList.remove('active');
            }
        }
    });
}