using System;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

class CyberSecurityAwarenessBot
{
    // User information storage for memory feature
    private static Dictionary<string, string> userMemory = new Dictionary<string, string>();

    // Delegate for keyword response handling
    private delegate string KeywordResponseHandler(string userInput);

    // Enhanced Neon Color Console Extension with more vibrant options
    private static class NeonColors
    {
        public static readonly ConsoleColor NeonPink = ConsoleColor.Magenta;
        public static readonly ConsoleColor NeonGreen = ConsoleColor.Green;
        public static readonly ConsoleColor NeonBlue = ConsoleColor.Blue;
        public static readonly ConsoleColor NeonYellow = ConsoleColor.Yellow;
        public static readonly ConsoleColor NeonCyan = ConsoleColor.Cyan;
        public static readonly ConsoleColor NeonRed = ConsoleColor.Red;
        public static readonly ConsoleColor SkyBlue = ConsoleColor.Cyan;
        public static readonly ConsoleColor NeonPurple = ConsoleColor.DarkMagenta;
        public static readonly ConsoleColor ElectricBlue = ConsoleColor.Blue;
        public static readonly ConsoleColor VibrantOrange = ConsoleColor.DarkYellow;
        public static readonly ConsoleColor LimeGreen = ConsoleColor.Green;
        public static readonly ConsoleColor HotPink = ConsoleColor.Magenta;
        public static readonly ConsoleColor AquaBlue = ConsoleColor.Cyan;
    }

    // Expanded Cybersecurity Topics Dictionary with additional topics
    private static Dictionary<int, (string Topic, string Description)> cyberTopics = new Dictionary<int, (string, string)>
    {
        {1, ("Password Security", "Learn how to create and manage strong, unbreakable passwords")},
        {2, ("Phishing Awareness", "Identify and protect yourself from online scams and fraudulent attempts")},
        {3, ("Social Media Safety", "Protect your personal information and privacy on social platforms")},
        {4, ("Two-Factor Authentication", "Understand the importance of additional security layers")},
        {5, ("WiFi and Network Security", "Safeguard your online connections and personal data")},
        {6, ("Email Safety", "Recognize and prevent email-based cyber threats")},
        {7, ("Personal Data Protection", "Strategies to keep your personal information secure online")},
        {8, ("Online Shopping Safety", "Protect yourself from fraud during online transactions")},
        {9, ("Malware Protection", "Learn to defend against malicious software and viruses")},
        {10, ("IoT Device Security", "Secure your smart home devices and connected technology")},
        {11, ("Public WiFi Dangers", "Navigate public networks safely without compromising data")},
        {12, ("Ransomware Defense", "Prevent and respond to ransomware attacks effectively")}
    };

    // Enhanced keyword recognition dictionary with expanded responses for each keyword
    private static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        {"password", new List<string>{
            "Creating strong passwords is essential. Use at least 16 characters with a mix of letters, numbers, and symbols.",
            "Remember to use a unique password for each account - never reuse passwords across multiple sites.",
            "Consider using a trusted password manager to keep track of all your unique passwords securely.",
            "Change your passwords regularly, especially for critical accounts like banking and email.",
            "Avoid using easily guessable information like birthdays or names in your passwords.",
            "A passphrase (multiple random words) can be more secure and easier to remember than a complex password.",
            "Many data breaches occur because of weak passwords. Your digital fortress is only as strong as its weakest access point.",
            "Password rotation policies (changing every 90 days) are now considered less effective than using strong, unique passwords with 2FA."
        }},
        {"phishing", new List<string>{
            "Always verify the sender's email address before clicking on links or downloading attachments.",
            "Legitimate organizations won't ask for sensitive information via email. When in doubt, contact them directly.",
            "Look for telltale signs like poor grammar, urgent requests, or suspicious URLs before interacting with emails.",
            "Hover over links to preview the actual URL before clicking to ensure it matches the expected destination.",
            "Be wary of emails creating a sense of urgency - phishers often use this tactic to pressure victims.",
            "Spear phishing targets specific individuals with personalized content - be especially cautious of emails that seem to know you.",
            "Modern phishing attempts are becoming increasingly sophisticated, sometimes mimicking trusted contacts perfectly.",
            "Voice phishing (vishing) and SMS phishing (smishing) are growing threats - apply the same caution to calls and texts."
        }},
        {"privacy", new List<string>{
            "Regularly review privacy settings on all your social media accounts to control what information is shared.",
            "Be mindful about what personal information you share online - once it's out there, it's difficult to take back.",
            "Consider using privacy-focused browsers and search engines for additional protection online.",
            "Limit the personal details you share in online profiles and public forums.",
            "Review and clear your browsing history and cookies regularly to minimize tracking.",
            "Use data minimization as a strategy - only provide the minimum information necessary for any service.",
            "Consider using temporary or alias email addresses for signups to reduce spam and tracking.",
            "Digital privacy is a continuous process, not a one-time setup. Regular privacy checkups are essential."
        }},
        {"scam", new List<string>{
            "If an offer seems too good to be true, it probably is. Trust your instincts when dealing with online offers.",
            "Verify the legitimacy of websites before making purchases by checking reviews and security certificates.",
            "Never send money or provide financial information to people or businesses you haven't verified.",
            "Be cautious of unsolicited communications requesting personal information or financial details.",
            "Research unfamiliar companies thoroughly before engaging in any transactions.",
            "Romance scams are increasingly common - be vigilant if online relationships progress quickly to requests for money.",
            "Cryptocurrency scams are on the rise - be extremely cautious of investment opportunities promising guaranteed returns.",
            "Tech support scams often begin with pop-up warnings or unexpected calls - legitimate companies don't reach out this way."
        }},
        {"virus", new List<string>{
            "Keep your antivirus software updated to protect against the latest threats.",
            "Be cautious about downloading files from unknown sources to avoid malware infection.",
            "Regularly scan your devices for malware and remove any suspicious programs immediately.",
            "Don't open email attachments from unknown senders as they may contain malicious code.",
            "Keep your operating system and applications updated with security patches.",
            "Fileless malware resides in memory rather than files - advanced security solutions are needed to detect them.",
            "Some modern viruses can hijack your webcam or microphone - consider covering cameras when not in use.",
            "Zero-day vulnerabilities are exploited before patches are available - layered security approaches offer best protection."
        }},
        {"vpn", new List<string>{
            "A VPN (Virtual Private Network) encrypts your internet connection for improved privacy.",
            "Always use a VPN when connecting to public WiFi networks to protect your data.",
            "Choose reputable VPN providers that don't log your activities for maximum privacy.",
            "A VPN can help bypass geo-restrictions but remember to use them ethically and legally.",
            "Some free VPNs may collect and sell your data, so research before selecting a provider.",
            "VPNs aren't perfect - they don't protect against all threats and can sometimes slow your connection.",
            "Consider using split tunneling to route only sensitive traffic through your VPN for better performance.",
            "Even with a VPN, practice good security habits - a VPN complements but doesn't replace other security measures."
        }},
        {"hack", new List<string>{
            "Enable automatic updates on all your devices to patch security vulnerabilities.",
            "Use different security questions for different accounts, and avoid using easily guessable answers.",
            "Monitor your accounts regularly for any suspicious activity that might indicate a breach.",
            "Enable login notifications when available to be alerted of unauthorized access attempts.",
            "Consider using hardware security keys for critical accounts for enhanced protection.",
            "Supply chain attacks target software suppliers - only download software from trusted sources.",
            "Social engineering is the most common hacking method - technical safeguards can't fully protect against human manipulation.",
            "Consider segmenting your digital life - use different emails for critical accounts versus signups and subscriptions."
        }},
        {"social media", new List<string>{
            "Adjust your privacy settings to control who can see your posts and personal information.",
            "Be selective about accepting friend or connection requests from people you don't know.",
            "Think twice before sharing your location, travel plans, or personal details publicly.",
            "Regularly audit which third-party apps have access to your social media accounts.",
            "Create strong, unique passwords for each social media platform you use.",
            "Beware of quizzes and games that request excessive permissions - they may be data harvesting operations.",
            "Social media accounts can be valuable targets for identity thieves - protecting them is protecting your identity.",
            "Consider using a professional account separate from your personal account for work-related networking."
        }},
        {"authentication", new List<string>{
            "Use two-factor authentication whenever possible to add an extra layer of security.",
            "Authenticator apps provide better security than SMS codes for two-factor authentication.",
            "Keep backup codes in a secure location in case you lose access to your authentication device.",
            "Consider using hardware security keys for the strongest form of authentication.",
            "Never share authentication codes with anyone, even if they claim to be from technical support.",
            "Biometric authentication (fingerprint, face recognition) offers convenience but isn't foolproof.",
            "Multi-factor authentication combines something you know, something you have, and sometimes something you are.",
            "Passwordless authentication is an emerging trend - security keys or biometrics may eventually replace passwords."
        }},
        {"wifi", new List<string>{
            "Always use a strong, unique password for your home WiFi network.",
            "Enable WPA3 encryption if your router supports it for better security.",
            "Disable remote management features unless absolutely necessary.",
            "Create a separate guest network for visitors to keep your main network secure.",
            "Regularly update your router's firmware to patch security vulnerabilities.",
            "Change default router login credentials immediately after setup.",
            "Hide your network SSID to make it slightly harder for casual attackers to find your network.",
            "Consider using MAC address filtering as an additional layer of network protection."
        }},
        {"malware", new List<string>{
            "Install reputable antimalware software and keep it updated with the latest definitions.",
            "Be cautious about downloading free software, which may contain bundled malware.",
            "Use caution when clicking on pop-up windows, which may contain malicious code.",
            "Regularly back up important data to protect against ransomware attacks.",
            "Configure your email client to block potentially dangerous attachment types.",
            "Advanced persistent threats (APTs) are sophisticated malware campaigns that may remain undetected for long periods.",
            "Some malware spreads via USB drives - be cautious when using removable storage from unknown sources.",
            "Rootkits operate at the system level and can be extremely difficult to detect and remove."
        }},
        {"iot", new List<string>{
            "Change default passwords on all IoT devices immediately after setup.",
            "Keep firmware updated on all smart home devices to patch security vulnerabilities.",
            "Consider using a separate network for IoT devices to isolate them from your main network.",
            "Research security features before purchasing IoT devices - not all manufacturers prioritize security.",
            "Disable features you don't use to reduce the potential attack surface.",
            "IoT devices often have minimal built-in security - your router is your first line of defense.",
            "Voice assistants may be always listening - review privacy settings and consider muting when not in use.",
            "Many IoT vulnerabilities remain unpatched - sometimes the most secure option is not connecting a device."
        }},
        {"ransomware", new List<string>{
            "Regular backups are your best defense against ransomware - ensure they're stored offline.",
            "Never pay the ransom - payment doesn't guarantee recovery and funds criminal activities.",
            "Keep software updated to prevent attackers from exploiting known vulnerabilities.",
            "Be especially cautious of email attachments, the primary ransomware delivery method.",
            "Consider using application whitelisting to prevent unauthorized software execution.",
            "Ransomware attacks are increasingly targeting specific high-value businesses rather than random victims.",
            "Some ransomware variants now steal data before encryption, adding the threat of exposure to the attack.",
            "Have an incident response plan ready before an attack occurs - knowing what to do saves critical time."
        }},
        {"browser", new List<string>{
            "Keep your browser updated to protect against security vulnerabilities.",
            "Use browser extensions that block trackers and unwanted scripts.",
            "Clear cookies and cache regularly to remove tracking data.",
            "Be cautious of browser notifications and permission requests from websites.",
            "Consider using incognito/private browsing mode for sensitive activities.",
            "Browser fingerprinting can track you even without cookies - specialized privacy tools may help.",
            "Some browser extensions can pose security risks - only install those with good reviews from trusted sources.",
            "Consider using different browsers for different activities to compartmentalize your digital footprint."
        }},
        {"children", new List<string>{
            "Use parental controls and content filtering to create a safer online environment.",
            "Teach children about online privacy and the permanence of what they share online.",
            "Keep communication open about online interactions and encourage children to report concerns.",
            "Set clear boundaries about screen time and appropriate online behavior.",
            "Monitor young children's online activities while gradually introducing more independence as they mature.",
            "Online predators often target children through games and social platforms - teach recognition of grooming tactics.",
            "Help children understand that people online may not be who they claim to be.",
            "Create family agreements about technology use and internet safety rules."
        }}
    };

    // Enhanced sentiment detection dictionary with more emotional states
    private static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        {"worried", "It's completely understandable to feel concerned about cybersecurity. Many threats exist, but with the right knowledge, you can significantly reduce your risk."},
        {"anxious", "I understand feeling anxious about online security. Let's break things down into manageable steps to help you feel more in control."},
        {"confused", "Cybersecurity can indeed be confusing with all the technical terms. I'll try to explain things clearly and simply."},
        {"overwhelmed", "It's easy to feel overwhelmed by cybersecurity information. Let's focus on the most important aspects first."},
        {"frustrated", "Dealing with security measures can be frustrating sometimes, but they're worth the effort to keep your information safe."},
        {"scared", "Being concerned about online threats is legitimate. I'm here to help you understand how to protect yourself effectively."},
        {"curious", "It's great that you're curious about cybersecurity! Learning more is the best way to stay protected online."},
        {"interested", "Your interest in cybersecurity is excellent! This knowledge will help keep you safer in our digital world."},
        {"helpful", "I'm glad you found this helpful! Cybersecurity knowledge is one of the best defenses against online threats."},
        {"unsure", "It's okay to be unsure about cybersecurity concepts. Let me guide you through the fundamentals in a clear way."},
        {"confident", "Your confidence is great! Just remember that cybersecurity requires ongoing vigilance as threats constantly evolve."},
        {"concerned", "Your concern about online security shows good awareness. Let's address your specific concerns with practical solutions."},
        {"happy", "I'm glad you're feeling positive! That enthusiasm will help you implement and maintain good security practices."},
        {"stressed", "Cybersecurity doesn't have to be stressful. We can break it down into simple, manageable steps."},
        {"vulnerable", "Many people feel vulnerable online. Taking consistent precautions will significantly improve your digital safety."},
        {"paranoid", "It's not paranoia when the threats are real! However, balanced security practices can help you feel more at ease."},
        {"excited", "Your excitement about learning cybersecurity is fantastic! This enthusiasm will serve you well in building strong defenses."},
        {"doubtful", "It's natural to feel doubtful about security measures. Let me help clarify which practices truly make a difference."},
        {"hopeful", "Your hopeful attitude is perfect! With the right knowledge and tools, you can absolutely improve your digital security."},
        {"cautious", "A cautious approach to cybersecurity is wise. That mindfulness will help you avoid many common threats."},
        {"optimistic", "I love your optimistic outlook! With good security habits, you can enjoy technology while staying safe."},
        {"empowered", "Feeling empowered is exactly right! Knowledge truly is power when it comes to cybersecurity."},
        {"intimidated", "Cybersecurity can seem intimidating at first, but we'll break it down into manageable concepts anyone can master."},
        {"relieved", "I'm glad you're feeling relieved! Building good security habits gives you peace of mind in the digital world."}
    };

    // Neon Cybersecurity Awareness Bot Logo - retaining original logo but with enhanced color display function
    private static void DisplayLogo()
    {
        // Create rainbow effect background cycle
        ConsoleColor[] rainbowColors = {
            NeonColors.NeonRed,
            NeonColors.VibrantOrange,
            NeonColors.NeonYellow,
            NeonColors.LimeGreen,
            NeonColors.ElectricBlue,
            NeonColors.NeonPurple,
            NeonColors.HotPink
        };

        // Animated background for logo introduction
        for (int i = 0; i < 3; i++)
        {
            foreach (var color in rainbowColors)
            {
                Console.BackgroundColor = color;
                Console.Clear();
                Thread.Sleep(70);
            }
        }

        Console.BackgroundColor = NeonColors.HotPink;
        Console.ForegroundColor = NeonColors.AquaBlue;

        Console.WriteLine(@"
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
******************************************************************************************************** ~
^             ^                         ^                                 ^                     ^      * ~
 ██████╗██╗   ██╗██████╗ ███████╗██████╗      ███████╗ █████╗ ███████╗███████╗████████╗██╗   ██╗       * ~
██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗     ██╔════╝██╔══██╗██╔════╝██╔════╝╚══██╔══╝╚██╗ ██╔╝       * ~
██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝     ███████╗███████║█████╗  █████╗     ██║    ╚████╔╝        * ~
██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗     ╚════██║██╔══██║██╔══╝  ██╔══╝     ██║     ╚██╔╝         * ~
╚██████╗   ██║   ██████╔╝███████╗██║  ██║     ███████║██║  ██║██║     ███████╗   ██║      ██║  ^       * ~
 ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝     ╚══════╝╚═╝  ╚═╝╚═╝     ╚══════╝   ╚═╝      ╚═╝          * ~
                 ^                              ^                                                   ^  * ~
        ^                   🔒 NEON CYBER GUARDIAN 3.0 🛡️          ^                                      * ~
                     ^                                                             ^                   * ~
   ^                                    ^                         ^                                  ^ * ~
******************************************************************************************************** ~
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
");

        // Animated border effect
        int borderEffectDuration = 5; // seconds
        DateTime endTime = DateTime.Now.AddSeconds(borderEffectDuration);

        while (DateTime.Now < endTime)
        {
            foreach (var color in rainbowColors)
            {
                if (DateTime.Now >= endTime) break;

                Console.ForegroundColor = color;
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~ QUANTUM SECURED CYBER DEFENSE SYSTEM ~~~~~~~~~~~~~~~~~~~~~~~~~");
                Thread.Sleep(150);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }

        Console.ResetColor();
    }

    // Enhanced Voice Greeting Simulation with visual effects
    private static void PlayVoiceGreeting()
    {
        try
        {
            // Fix: Add a proper check to see if the file exists before attempting to load it
            string audioFilePath = "Cyber (online-audio-converter.com).wav";
            if (System.IO.File.Exists(audioFilePath))
            {
                SoundPlayer soundPlayer = new SoundPlayer("Cyber (online-audio-converter.com).wav");
                soundPlayer.Load();
                soundPlayer.PlaySync();
            }
            else
            {
                throw new System.IO.FileNotFoundException("Audio file not found");
            }
        }
        catch (Exception)
        {
            // Gracefully handle missing audio file or other sound issues
            Console.ForegroundColor = NeonColors.NeonYellow;
            Console.WriteLine("[🔊 Audio system notice: Continuing in enhanced visual mode]");
            Console.ResetColor();
        }

        // Matrix-like digital rain effect
        Console.ForegroundColor = NeonColors.LimeGreen;
        Random rnd = new Random();
        for (int i = 0; i < 5; i++)
        {
            string digitalRain = "";
            for (int j = 0; j < 75; j++)
            {
                // Create random digital characters
                char c = (char)rnd.Next(33, 126);
                digitalRain += c;
            }
            Console.WriteLine(digitalRain);
            Thread.Sleep(100);
        }
        Console.ResetColor();

        Console.ForegroundColor = NeonColors.AquaBlue;
        Console.WriteLine(" ⚡⚡⚡ HELLO :) WELCOME TO CYBER SECURITY AWARENESS CHATBOT 3.0!! ⚡⚡⚡");
        Console.WriteLine(" 🌐 MY NAME IS CYBER SAFETY, PLEASE ENTER YOURS TO CONTINUE. 🌐");
        Console.ResetColor();

        Console.ForegroundColor = NeonColors.ElectricBlue;
        Console.WriteLine("\n[🔊 Quantum Voice Activation System v3.0]");
        Console.ResetColor();

        string[] voiceSounds = {
            "⚡ Neon Circuits Initializing...",
            "🌈 Cyber Wavelengths Calibrating...",
            "🔮 Holographic Security Interface Online!",
            "🛡️ Quantum Encryption Protocols Activated",
            "🔍 Cyberthreat Neural Analysis Ready",
            "✨ Digital Defense Matrix Connected"
        };

        // Animated loading bar
        Console.Write("\n[");
        for (int i = 0; i < 30; i++)
        {
            Console.ForegroundColor = i % 2 == 0 ? NeonColors.NeonCyan : NeonColors.HotPink;
            Console.Write("■");
            Thread.Sleep(70);
        }
        Console.WriteLine("]");
        Console.ResetColor();

        // Animated voice system initialization
        foreach (var sound in voiceSounds)
        {
            Console.ForegroundColor = NeonColors.NeonCyan;
            Console.Write(sound);

            // Add animated ellipsis
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(150);
            }

            Console.ForegroundColor = NeonColors.LimeGreen;
            Console.WriteLine(" ✓");
            Console.ResetColor();
            Thread.Sleep(250);
        }

        // Final activation sequence
        Console.ForegroundColor = NeonColors.NeonYellow;
        Console.WriteLine("\n[🚀 SYSTEM FULLY ACTIVATED - READY TO PROTECT]");
        Console.ResetColor();
    }

    // Enhanced Topics Menu with visual categories
    private static void DisplayTopicsMenu()
    {
        // Display a cyber shield symbol at the top of the menu
        Console.ForegroundColor = NeonColors.NeonYellow;
        Console.WriteLine(@"
                    /\
                   /  \
                  /    \
                 /      \
                /========\
               /|        |\
              / |   🔒   | \
             /__|________|__\
        ");
        Console.ResetColor();

        Console.ForegroundColor = NeonColors.HotPink;
        Console.WriteLine("\n┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
        Console.WriteLine("┃                              🌐 CYBER SAFETY TOPICS MENU 🛡️                                        ┃");
        Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
        Console.ResetColor();

        // Group topics by category for better organization
        var personalSecurity = new List<int> { 1, 4, 7 };
        var onlineInteraction = new List<int> { 2, 3, 6, 8 };
        var networkSafety = new List<int> { 5, 9, 10, 11, 12 };

        // Display personal security topics
        Console.ForegroundColor = NeonColors.NeonCyan;
        Console.WriteLine("\n🔐 PERSONAL SECURITY");
        Console.ResetColor();

        foreach (var topicIndex in personalSecurity)
        {
            var topic = cyberTopics[topicIndex];
            Console.ForegroundColor = NeonColors.ElectricBlue;
            Console.WriteLine($"  {topicIndex}. {topic.Topic}");
            Console.ForegroundColor = NeonColors.NeonBlue;
            Console.WriteLine($"     {topic.Description}\n");
        }

        // Display online interaction topics
        Console.ForegroundColor = NeonColors.NeonCyan;
        Console.WriteLine("🌐 ONLINE INTERACTIONS");
        Console.ResetColor();

        foreach (var topicIndex in onlineInteraction)
        {
            var topic = cyberTopics[topicIndex];
            Console.ForegroundColor = NeonColors.ElectricBlue;
            Console.WriteLine($"  {topicIndex}. {topic.Topic}");
            Console.ForegroundColor = NeonColors.NeonBlue;
            Console.WriteLine($"     {topic.Description}\n");
        }

        // Display network safety topics
        Console.ForegroundColor = NeonColors.NeonCyan;
        Console.WriteLine("📡 NETWORK & DEVICE SAFETY");
        Console.ResetColor();

        foreach (var topicIndex in networkSafety)
        {
            var topic = cyberTopics[topicIndex];
            Console.ForegroundColor = NeonColors.ElectricBlue;
            Console.WriteLine($"  {topicIndex}. {topic.Topic}");
            Console.ForegroundColor = NeonColors.NeonBlue;
            Console.WriteLine($"     {topic.Description}\n");
        }

        // Navigation options
        Console.ForegroundColor = NeonColors.HotPink;
        Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
        Console.WriteLine("┃                                      NAVIGATION OPTIONS                                          ┃");
        Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
        Console.ResetColor();

        Console.ForegroundColor = NeonColors.NeonGreen;
        Console.WriteLine("  C. 💬 Chat with Cyber Guardian");
        Console.ForegroundColor = NeonColors.VibrantOrange;
        Console.WriteLine("  T. 🧪 Test Your Cybersecurity Knowledge");
        Console.ForegroundColor = NeonColors.NeonRed;
        Console.WriteLine("  0. 🚪 Exit Cyber Safety Guardian");
        Console.ResetColor();
    }

    // Enhanced Topic Details with visual elements and expanded content
    private static void ShowTopicDetails(int topicChoice)
    {
        // Create a dictionary mapping topic choices to their detailed information
        Dictionary<int, string> topicDetails = new Dictionary<int, string>
        {
            {1, @"🔐 ADVANCED PASSWORD SECURITY:
- Create passwords 16+ characters long
- Use a mix of uppercase, lowercase, numbers, symbols
- Avoid personal information (birthdays, names)
- Use unique passwords for each account
- Utilize password manager
- Enable password strength checkers
- Consider passphrase method
- Use biometric authentication when available
- Set up account recovery options
- Regularly check for data breaches
- Never share passwords, even with trusted people
- Be wary of keyloggers on public computers"},

            {2, @"🎣 PHISHING DEFENSE STRATEGIES:
- Verify sender's email address carefully
- Look for suspicious links or attachments
- Check for spelling and grammatical errors
- Never share personal info via email
- Use email filtering tools
- Implement anti-phishing browser extensions
- Report suspicious emails to IT or platform
- Be especially cautious of urgent requests
- Verify requests through alternate channels
- Enable spam filtering on all accounts
- Educate family members about phishing risks
- Watch for sign-in attempts from new locations"},

            {3, @"🌐 SOCIAL MEDIA PROTECTION:
- Set profiles to private
- Be selective with friend/connection requests
- Limit personal information shared
- Use strong, unique passwords
- Enable two-factor authentication
- Review app permissions regularly
- Be cautious of location sharing
- Think before posting personal content
- Understand platform privacy policies
- Regularly review tagged photos
- Check which third-party apps have access
- Create separate accounts for different purposes
- Be cautious about social media quizzes"},

            {4, @"🔒 TWO-FACTOR AUTHENTICATION GUIDE:
- Prefer authenticator apps over SMS
- Use multiple 2FA methods when possible
- Keep backup codes in secure location
- Avoid using the same 2FA method everywhere
- Update 2FA settings regularly
- Consider hardware security keys
- Be aware of 2FA recovery processes"},

            {5, @"📡 NETWORK AND WIFI SECURITY:
- Use VPN on public networks
- Disable auto-connect to unknown networks
- Use WPA3 encryption at home
- Change router default passwords
- Update router firmware
- Use guest networks for visitors
- Disable remote management
- Use MAC address filtering"},

            {6, @"📧 EMAIL SAFETY PROTOCOLS:
- Use email encryption
- Be wary of unexpected attachments
- Use separate emails for different purposes
- Enable spam and phishing filters
- Use disposable email for online registrations
- Regularly clean up email subscriptions
- Be cautious of urgent or threatening emails
- Verify sender's identity"},

            {7, @"🔐 PERSONAL DATA PROTECTION:
- Minimize personal info shared online
- Use privacy-focused browsers
- Regularly check privacy settings
- Be cautious of online quizzes and surveys
- Use privacy protection services
- Monitor credit reports
- Use encrypted messaging apps
- Be careful with online forms"},

            {8, @"💳 ONLINE SHOPPING SECURITY:
- Shop on secure, verified websites
- Look for HTTPS and lock icon
- Use credit cards over debit
- Enable transaction alerts
- Use virtual credit card numbers
- Avoid public WiFi for transactions
- Check website authenticity
- Use trusted payment methods
- Keep software and browser updated"}
        };

        // Get the detailed information or provide a default message
        string detailedInfo = topicDetails.ContainsKey(topicChoice)
            ? topicDetails[topicChoice]
            : "Invalid topic selection. Please choose a valid option.";

        Console.ForegroundColor = NeonColors.NeonCyan;
        Console.WriteLine($"\n🔒 DETAILED CYBER INTELLIGENCE FOR TOPIC:");
        Console.ForegroundColor = NeonColors.NeonBlue;
        Console.WriteLine(detailedInfo);
        Console.ResetColor();
    }

    // Enhanced keyword recognition and response generator using delegate
    private static KeywordResponseHandler keywordResponseDelegate = (userInput) =>
    {
        // Handle null userInput gracefully
        if (string.IsNullOrEmpty(userInput))
        {
            return "I'm waiting for your cybersecurity question. You can ask about topics like passwords, phishing, privacy, and more.";
        }

        // Create a list to hold potential matching keywords
        List<string> matchedKeywords = new List<string>();

        // Find all keywords that match in the user input
        foreach (var keyword in keywordResponses.Keys)
        {
            if (Regex.IsMatch(userInput, $"\\b{keyword}\\b", RegexOptions.IgnoreCase))
            {
                matchedKeywords.Add(keyword);
            }
        }

        // If multiple matches found, prioritize the longer keyword (more specific)
        if (matchedKeywords.Count > 0)
        {
            // Sort by length descending to prioritize longer, more specific keywords
            matchedKeywords.Sort((a, b) => b.Length.CompareTo(a.Length));

            // Get a random response for the highest priority keyword
            Random random = new Random();
            List<string> responses = keywordResponses[matchedKeywords[0]];
            return responses[random.Next(responses.Count)];
        }

        // Default response if no keyword matched
        return "I'm not sure I understand that cybersecurity topic. Could you try rephrasing or ask about passwords, phishing, privacy, scams, viruses, VPNs, or hacking?";
    };

    // Enhanced sentiment detection function
    private static string DetectSentiment(string userInput)
    {
        // Handle null userInput gracefully
        if (string.IsNullOrEmpty(userInput))
        {
            return string.Empty;
        }

        // Create a list to hold potential matching sentiments
        List<string> matchedSentiments = new List<string>();

        foreach (var sentiment in sentimentResponses.Keys)
        {
            if (Regex.IsMatch(userInput, $"\\b{sentiment}\\b", RegexOptions.IgnoreCase))
            {
                matchedSentiments.Add(sentiment);
            }
        }

        // If multiple matches found, choose one randomly for variety
        if (matchedSentiments.Count > 0)
        {
            Random random = new Random();
            string selectedSentiment = matchedSentiments[random.Next(matchedSentiments.Count)];
            return sentimentResponses[selectedSentiment];
        }

        // No specific sentiment detected
        return string.Empty;
    }

    // Store user information in memory
    private static void StoreUserInfo(string key, string value)
    {
        if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
        {
            userMemory[key.ToLower()] = value;
        }
    }

    // Enhanced chat interface with improved conversation flow
    private static void ChatWithCyberGuardian(string userName)
    {
        Console.ForegroundColor = NeonColors.NeonCyan;
        Console.WriteLine($"\n🤖 CYBER GUARDIAN CHAT MODE ACTIVATED");
        Console.WriteLine($"Hello {userName}! You can chat with me about any cybersecurity topic. Type 'menu' to return to the main menu.\n");
        Console.ResetColor();

        StoreUserInfo("name", userName);

        bool chatActive = true;
        string previousTopic = string.Empty;
        int conversationTurns = 0;

        while (chatActive)
        {
            Console.ForegroundColor = NeonColors.NeonGreen;
            Console.Write($"{userName}: ");
            Console.ResetColor();

            string userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                continue;
            }

            if (userInput.Trim().Equals("menu", StringComparison.OrdinalIgnoreCase))
            {
                chatActive = false;
                continue;
            }

            conversationTurns++;

            // Store user interests for future reference
            if (userInput.IndexOf("interested in", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                int interestIndex = userInput.IndexOf("interested in", StringComparison.OrdinalIgnoreCase);
                if (interestIndex >= 0)
                {
                    string interest = userInput.Substring(interestIndex + 13).Trim();
                    if (!string.IsNullOrWhiteSpace(interest))
                    {
                        StoreUserInfo("interest", interest);
                        Console.ForegroundColor = NeonColors.NeonCyan;
                        Console.WriteLine($"Cyber Guardian: Great! I'll remember that you're interested in {interest}. It's a crucial part of staying safe online.");
                        Console.ResetColor();
                        previousTopic = interest;
                        continue;
                    }
                }
            }

            // Enhanced memory features - detect and store additional user preferences
            foreach (var topic in cyberTopics)
            {
                if (userInput.IndexOf(topic.Value.Topic, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    StoreUserInfo("last_topic", topic.Value.Topic);
                    break;
                }
            }

            // Detect sentiment in user input
            string sentimentResponse = DetectSentiment(userInput);

            Console.ForegroundColor = NeonColors.NeonCyan;
            Console.Write("Cyber Guardian: ");

            // Handle sentiment if detected
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                Console.WriteLine(sentimentResponse);
            }

            // Main response based on keyword recognition using delegate
            string keywordResponse = keywordResponseDelegate(userInput);
            Console.WriteLine(keywordResponse);

            // Reference previous topics or user interests when appropriate (improved memory feature)
            if (userMemory.ContainsKey("interest") && (conversationTurns % 3 == 0) && string.IsNullOrEmpty(sentimentResponse))
            {
                Console.WriteLine($"Since you're interested in {userMemory["interest"]}, you might also want to look into related protective measures.");
            }

            // Use other memory elements to personalize the conversation
            if (userMemory.ContainsKey("last_topic") && userMemory["last_topic"] != previousTopic && (new Random().Next(4) == 0))
            {
                Console.WriteLine($"By the way, regarding {userMemory["last_topic"]} that we discussed earlier, remember that it's an essential aspect of your overall cybersecurity posture.");
            }

            Console.ResetColor();
            previousTopic = userInput;
        }
    }

    // Main Interaction with improved error handling
    private static void StartChatSession(string userName)
    {
        // Validate username input
        userName = string.IsNullOrWhiteSpace(userName) ? "Cyber Agent" : userName;
        StoreUserInfo("name", userName);

        Console.ForegroundColor = NeonColors.NeonGreen;
        Console.WriteLine($"\n👤 Welcome, {userName}! How are you doing today?");
        Console.ResetColor();

        string userMood = Console.ReadLine();
        string sentimentResponse = DetectSentiment(userMood);

        // Store initial mood in memory
        if (!string.IsNullOrEmpty(userMood))
        {
            StoreUserInfo("initial_mood", userMood);
        }

        if (!string.IsNullOrEmpty(sentimentResponse))
        {
            Console.ForegroundColor = NeonColors.NeonCyan;
            Console.WriteLine($"Cyber Guardian: {sentimentResponse}");
            Console.ResetColor();
        }

        while (true)
        {
            DisplayTopicsMenu();

            Console.ForegroundColor = NeonColors.NeonYellow;
            Console.Write("\n🔍 Enter topic number, 'C' to chat, or '0' to exit: ");
            Console.ResetColor();

            string userChoice = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userChoice))
            {
                continue;
            }

            if (userChoice.Equals("0", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = NeonColors.NeonPink;

                // Use memory to personalize exit message
                if (userMemory.ContainsKey("interest"))
                {
                    Console.WriteLine($"\n🛡️ Stay safe online, {userName}! Keep learning about {userMemory["interest"]} to enhance your cybersecurity knowledge. Cyber Guardian logging out... 🌈");
                }
                else
                {
                    Console.WriteLine($"\n🛡️ Stay safe online, {userName}! Cyber Guardian logging out... 🌈");
                }
                break;
            }

            if (userChoice.Equals("C", StringComparison.OrdinalIgnoreCase))
            {
                ChatWithCyberGuardian(userName);
                continue;
            }

            if (!int.TryParse(userChoice, out int topicChoice))
            {
                Console.ForegroundColor = NeonColors.NeonRed;
                Console.WriteLine("Invalid input. Please enter a number, 'C' to chat, or '0' to exit.");
                Console.ResetColor();
                continue;
            }

            if (cyberTopics.ContainsKey(topicChoice))
            {
                ShowTopicDetails(topicChoice);

                // Store user topic selection in memory
                StoreUserInfo("last_selected_topic", cyberTopics[topicChoice].Topic);
            }
            else
            {
                Console.ForegroundColor = NeonColors.NeonRed;
                Console.WriteLine("Invalid topic number. Please choose from the menu.");
                Console.ResetColor();
            }

            Console.ForegroundColor = NeonColors.NeonGreen;
            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }

    static void Main(string[] args)
    {
        try
        {
            Console.Title = "Neon Cyber Safety Guardian 2.0";
        }
        catch
        {
            // Silently handle if console title can't be set
        }

        try
        {
            // Fix: Avoid potential issue with setting window width on some platforms
            if (OperatingSystem.IsWindows())
            {
                Console.WindowWidth = Math.Max(Console.WindowWidth, 120);
            }
        }
        catch
        {
            // Handle case where console window size cannot be changed
            Console.WriteLine("Notice: For best experience, maximize your console window.");
        }

        try
        {
            DisplayLogo();
            PlayVoiceGreeting();

            Console.ForegroundColor = NeonColors.NeonGreen;
            Console.Write("\n👤 CYBER AGENT IDENTIFICATION > ");
            Console.ResetColor();

            string userName = Console.ReadLine();
            StartChatSession(userName);
        }
        catch (Exception ex)
        {
            // Global exception handler
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[SYSTEM ERROR] Something went wrong. Please restart the application.");
            Console.WriteLine("Error details (for debugging): " + ex.Message);
            Console.ResetColor();

            // Wait for user acknowledgment before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    // Extension method to check if running on Windows
    private static class OperatingSystem
    {
        public static bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT ||
                   Environment.OSVersion.Platform == PlatformID.Win32Windows ||
                   Environment.OSVersion.Platform == PlatformID.Win32S ||
                   Environment.OSVersion.Platform == PlatformID.WinCE;
        }
    }
}